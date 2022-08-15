using Microsoft.Extensions.Configuration;
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

namespace OmniChannel.Warehouses
{
    [RemoteService(true)]
    public class WarehouseAppService : OmniChannelAppService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ShareAppService _shareAppService;
        private readonly string TiktokShopApiGetListWareHouse = OmniChannelConsts.TiktokShopAPIGetListWareHouse;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly HttpClientAppService _httpClientAppService;
        public WarehouseAppService(HttpClient httpClient,
            IConfiguration configuration,
            ShareAppService shareAppService,
            HttpClientAppService httpClientAppService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _shareAppService = shareAppService;
            _httpClientAppService = httpClientAppService;
        }
        public async Task<ResponseDataDto<WareHouseListDto>> GetListWareHouse(string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListWareHouse, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            var a = await httpResponseMessage.Content.ReadAsStringAsync();

            var tets = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<WareHouseListDto>>(httpResponseMessage);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<WareHouseListDto>>(httpResponseMessage);
        }
        //public async Task<List<WareHouseListDto>> GetListWareHouseByIdShop(string idShop)
        //{
        //    var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListWareHouse, app_key, access_token, idShop, app_secret, new List<string>());
        //    var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

        //    var result = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<WareHouseListDto>>(httpResponseMessage);
        //    return List<result.data>;
        //}
    }
}
