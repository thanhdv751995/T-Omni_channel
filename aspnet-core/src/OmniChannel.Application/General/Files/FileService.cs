
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMT.Authentications.Authentications;
using Volo.Abp;

namespace OmniChannel.Files
{
    [RemoteService(IsEnabled = false)]
    public class FileService : OmniChannelAppService, IFileService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServerAuthService _serverAuthService;
        private readonly IConfiguration _configuration;
        public FileService(
        IHttpClientFactory httpClientFactory,
        IServerAuthService serverAuthService,
        IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _serverAuthService = serverAuthService;
            _configuration = configuration;
        }

        private static void AddHeader(HttpClient http, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    http.DefaultRequestHeaders.Add(header.Key, header.Value);

                }
            }
        }
        public Base64FileDto ConvertToBase64(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string base64Content = Convert.ToBase64String(fileBytes);
                // act on the Base64 data
                var fileType = Path.GetExtension(file.FileName);
                return new Base64FileDto { Content = base64Content, Extention = fileType, Size = file.Length };
            }
            throw new BusinessException("that bai");
        }

        public IEnumerable<Base64FileDto> ConvertToBase64(IList<IFormFile> files)
        {
            var rs = new List<Base64FileDto>();
            foreach (var file in files)
            {
                var base64Dto = ConvertToBase64(file);
                rs.Add(base64Dto);
            }
            return rs;
        }

        public async Task<IEnumerable<UrlFileDto>> UploadFileAsync(IList<IFormFile> files)
        {
            // result
            var result = new List<UrlFileDto>();
            // create fromdata
            var formContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                formContent.Add(new StreamContent(file.OpenReadStream()), "files", file.FileName);
            }

            //// handler httpclient
            // get token
            var host = _configuration["Services:FileServer:Host"].RemovePostFix("/");
            //var serverAuth = await _serverAuthService.LoginAsync();
            var client = _httpClientFactory.CreateClient();
            //// add token to header
            //AddHeader(client, new Dictionary<string, string> { { "Authorization", $"{serverAuth.TokenType} {serverAuth.AccessToken}" } });
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = formContent,
                RequestUri = new Uri($"{host}/api/v1/upload")
            };
            var response = await client.SendAsync(request);
            var contenString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var uploadResult = JsonConvert.DeserializeObject<List<string>>(contenString).ToArray();
                var i = 0;
                foreach (var file in files)
                {
                    result.Add(new UrlFileDto { Content = uploadResult[i], Extention = Path.GetExtension(file.FileName), Size = file.Length, FileName = file.FileName });
                    i++;
                }
                return result;

            }
            throw new BusinessException("Upload file is fail");
        }

        public async Task<UrlFileDto> UploadFileAsync(IFormFile file)
        {
            var result = await UploadFileAsync(new List<IFormFile> { file });
            return result.FirstOrDefault();
        }
    }
}