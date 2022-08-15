using Microsoft.AspNetCore.Components;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Shopee.Warehouses
{
    [RemoteService(false)]
    public class WarehouseAppService: OmniChannelAppService
    {
        private readonly string API_GET_WAREOUSE_DETAIL = ShopeeConst.API_GET_WAREOUSE_DETAIL;

        private readonly ShareAppService _shareAppService;

        public WarehouseAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<string> GetWarehouseDetail(int shop_id, string access_token)
        {
            var url = _shareAppService.GetShopeeUrl(API_GET_WAREOUSE_DETAIL, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
