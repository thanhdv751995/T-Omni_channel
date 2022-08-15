using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using Volo.Abp;
using Microsoft.Extensions.Configuration;

namespace OmniChannel.Brands
{
    [RemoteService(true)]
    public class BrandAppService : OmniChannelAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly string TiktokShopApiGetListBrand = OmniChannelConsts.TiktokShopApiGetListBrand;
        private readonly IConfiguration _configuration;
        public BrandAppService(ShareAppService shareAppService, IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }
        public async Task<ResponseDataDto<ResponseDataListBrandDto>> GetBrands(string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListBrand, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
               channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseDataListBrandDto>>(httpResponseMessage);
        }
    }
}
