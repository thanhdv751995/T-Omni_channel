using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmniChannel.General.Images;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using OmniChannel.Shopee.ResponseData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.Shopee.Images
{
    public class ImageSPAppService : OmniChannelAppService
    {
        private readonly string API_UPLOAD_IMAGE = ShopeeConst.API_UPLOAD_IMAGE;
        private readonly string IMAGE_URL_REGION = ShopeeConst.IMAGE_URL_REGION;

        private readonly ShareAppService _shareAppService;

        public ImageSPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<ResponseGUploadImageDto> UploadImage(IFormFile file)
        {
            var url = _shareAppService.GetShopeeUrlPublicAPI(API_UPLOAD_IMAGE, new List<string>() { });

            var httpResponseMessage = await SendRequestAsync(url, file);

            var response = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseImageInfoDto>>(httpResponseMessage);

            response.response.image_info.image_url_list = response.response.image_info.image_url_list.Where(x => x.image_url_region == IMAGE_URL_REGION).ToList();

            return new ResponseGUploadImageDto()
            {
                image_id = response.response.image_info.image_id,
                image_url = response.response.image_info.image_url_list[0].image_url
            };
        }

        private static async Task<HttpResponseMessage> SendRequestAsync(string url, IFormFile file)
        {
            using var _httpClient = new HttpClient();

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(file.OpenReadStream()), "image", file.FileName },

                // normal: Shopee will process the image as a square image, it is recommended to use when uploading item image
                { new StringContent("scene"), "normal" }
            };

            HttpRequestMessage request = new()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };

            var httpResponseMessage = await _httpClient.SendAsync(request);

            return httpResponseMessage;
        }
    }
}
