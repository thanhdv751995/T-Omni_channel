using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.Files;
using OmniChannel.MultiTenancy;
using Volo.Abp;
using OmniChannel.TiktokShop.Shares;
using OmniChannel.ChannelAuthentications;
using System.Linq.Expressions;
using Newtonsoft.Json;
using OmniChannel.Clients;
using OmniChannel.TiktokShop.CreateProducts;
using OmniChannel.CreateProducts;
using OmniChannel.Products;

namespace OmniChannel.Shares
{
    public class ShareAppService : OmniChannelAppService
    {
        private readonly string TiktokShopShopDomain = OmniChannelConsts.TiktokShopShopDomain;
        private readonly string SHOPEE_DOMAIN_URL = ShopeeConst.DOMAIN_URL;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly IConfiguration _configuration;

        public ShareAppService(IChannelAuthenticationRepository channelAuthenticationRepository,
            IConfiguration configuration)
        {
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _configuration = configuration;
        }

        public static string GetTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
        }

        public string GetSignatureAlgorithmTTS(string app_secret, string url)
        {
            string result = "";
            var urlSplit = url.Split('&');
            var requestPath = "";

            foreach (var key in urlSplit)
            {
                if (key.Contains('?'))
                {
                    result += key.Split('?')[1];
                    requestPath = key.Split('?')[0].Replace($"{TiktokShopShopDomain}", "");
                }

                if (!key.Contains("sign") && !key.Contains("access_token") && !key.Contains('?'))
                {
                    result += key;
                }
            }

            var stringGenerated = $"{app_secret}{requestPath}{result.Replace("=", "")}{app_secret}";

            var signature = CalcHMACSHA256Hash(stringGenerated, app_secret);

            return signature;
        }

        public static string CalcHMACSHA256Hash(string plaintext, string salt)
        {
            string result = "";

            var enc = Encoding.Default;
            byte[] baText2BeHashed = enc.GetBytes(plaintext),
            baSalt = enc.GetBytes(salt);
            HMACSHA256 hasher = new(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());

            return result;
        }

        public static string GetHMAC(string text, string key)
        {
            string sign = "";
            byte[] k = Encoding.ASCII.GetBytes(key)
;
            HMACSHA256 myhmacsha256 = new(k);
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            using (MemoryStream stream = new(byteArray))
            {
                sign = ByteToHex(myhmacsha256.ComputeHash(stream));
            }
            return sign;
        }

        public static string ByteToHex(byte[] byteArray)
        {
            StringBuilder result = new();
            foreach (byte b in byteArray)
            {
                result.AppendFormat("{0:x2}", b);
            }
            return result.ToString();
        }

        public static string GetParameters(bool isSignatureAlgorithm, string app_key, string access_token, string sign, string shopId, List<string> requestParameters)
        {
            string commonParameters;

            if (shopId != "")
            {
                commonParameters = isSignatureAlgorithm ? $"?app_key={app_key}&shop_id={shopId}&timestamp={GetTimestamp()}"
                                             : $"?app_key={app_key}&access_token={access_token}&sign={sign}&shop_id={shopId}&timestamp={GetTimestamp()}";
            }
            else
            {
                commonParameters = isSignatureAlgorithm ? $"?app_key={app_key}&timestamp={GetTimestamp()}"
                                             : $"?app_key={app_key}&access_token={access_token}&sign={sign}&timestamp={GetTimestamp()}";
            }


            foreach (var parameter in requestParameters)
            {
                commonParameters += parameter;
            }

            List<string> commonParametersSort = commonParameters.Replace("?", "").Split("&").ToList();

            commonParametersSort.Sort();

            return $"?{string.Join("&", commonParametersSort)}";
        }

        public string GetTiktokShopUrl(string urlApi, string app_key, string access_token, string shopId, string app_secret, List<string> requestParameters)
        {
            var sign = "";

            var url = $"{TiktokShopShopDomain}{urlApi}{GetParameters(true, app_key, access_token, sign, shopId, requestParameters)}";

            sign = GetSignatureAlgorithmTTS(app_secret, url);

            url = $"{TiktokShopShopDomain}{urlApi}{GetParameters(false, app_key, access_token, sign, shopId, requestParameters)}";

            return url;
        }

        #region Image

        //convert file to base64
        public static Base64FileDto ConvertToBase64(IFormFile file)
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
            throw new BusinessException("Fail");
        }

        #endregion

        public static List<string> GetRequestParameters(params dynamic[] values)
        {
            List<string> requestParameters = new();

            foreach (dynamic value in values)
            {
                if (value.Split("=")[1] != "")
                {
                    requestParameters.Add($"&{value.ToString()}");
                }
            }

            return requestParameters;
        }

        public async Task<ChannelAuthentication> GetAccessTokenShopId(string channel_token)
        {
            var channelAuthentication = await _channelAuthenticationRepository.FindAsync(x => x.Channel_token == channel_token);

            return channelAuthentication;
        }

        #region Shopee

        public string GetSignatureAlgorithmShopee(string url_api_path, long shop_id, string access_token, bool is_public_api)
        {
            string url;

            if (is_public_api)
            {
                url = string.Format("{0}{1}{2}", _configuration["ShopeeSetting:Partner_id"], url_api_path, GetTimestamp());
            }
            else
            {
                url = $"{_configuration["ShopeeSetting:Partner_id"]}{url_api_path}{GetTimestamp()}{access_token}{shop_id}";
            }

            return CalcHMACSHA256Hash(url, _configuration["ShopeeSetting:Key"]);
        }

        public string GetShopeeUrl(string url_api_path, long shop_id, string access_token, List<string> requestParameters)
        {
            var commonParameters = $"?partner_id={_configuration["ShopeeSetting:Partner_id"]}" +
                $"&timestamp={GetTimestamp()}" +
                $"&access_token={access_token}" +
                $"&shop_id={shop_id}" +
                $"&sign={GetSignatureAlgorithmShopee(url_api_path, shop_id, access_token, false)}";

            var url = $"{SHOPEE_DOMAIN_URL}{url_api_path}{ConvertParametersShopee(commonParameters, requestParameters)}";

            return url;
        }

        public string GetShopeeUrlPublicAPI(string url_api_path, List<string> requestParameters)
        {
            var commonParameters = $"?partner_id={_configuration["ShopeeSetting:Partner_id"]}" +
                $"&timestamp={GetTimestamp()}" +
                $"&sign={GetSignatureAlgorithmShopee(url_api_path, 0, string.Empty, true)}";

            var url = $"{SHOPEE_DOMAIN_URL}{url_api_path}{ConvertParametersShopee(commonParameters, requestParameters)}";

            return url;
        }

        public static string ConvertParametersShopee(string commonParameters, List<string> requestParameters)
        {
            foreach (var parameter in requestParameters)
            {
                commonParameters += parameter;
            }

            List<string> commonParametersSort = commonParameters.Replace("?", "").Split("&").ToList();

            return $"?{string.Join("&", commonParametersSort)}";
        }

        public static List<string> GetRequestParametersShopee(params dynamic[] values)
        {
            List<string> requestParameters = new();

            foreach (dynamic value in values)
            {
                requestParameters.Add($"&{value.ToString()}");
            }

            return requestParameters;
        }

        #endregion

        #region Enum

        public static string GetEnumName<T>(object value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        #endregion

        #region Convert

        public T ConvertObjToDto<T>(object obj)
        {
            return ObjectMapper.Map<object, T>(obj);
        }

        public object ConvertDtoToObj<T>(T dynamicDto)
        {
            return ObjectMapper.Map<T, object>(dynamicDto);
        }

        public static string ConvertObjToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ConvertDtoToString<T>(T dynamicDto)
        {
            return JsonConvert.SerializeObject(dynamicDto);
        }

        public static object ConvertStringToObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public T ConvertStringToDto<T>(string value)
        {
            return ConvertObjToDto<T>(ConvertStringToObject(value));
        }

        public T1 ConvertDtoToDto<T1, T2>(T2 dynamicDto)
        {
            return ObjectMapper.Map<T2, T1>(dynamicDto);
        }

        public static long GetTimestampTest()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(DateTime.Now - startTime).TotalSeconds;
        }

        //check  TTS và Tpos xem value của biến thể
        public static string CheckVariantProduct(List<AttributeValueDto> clientVariantDto, List<SalesAttributesDto> dataChannelProductDto)
        {
            string result = "";
            foreach (var client_data in clientVariantDto)
            {
                if (!dataChannelProductDto.Any(x => x.Custom_value.ToUpper() == client_data.Name.ToUpper()))
                {
                    throw new BusinessException("Không tồn tại giá trị biến thể này") ;
                }
                else
                {
                    result = " Đồng bộ thành công";
                }    

            }
            return result;
        }
        #endregion
    }
}
