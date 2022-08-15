using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Shopee.Shops
{
    [RemoteService(false)]
    public class ShopAppService : OmniChannelAppService
    {
        private readonly string API_GET_SHOP_INFO = ShopeeConst.API_GET_SHOP_INFO;
        private readonly string API_GET_SHOP_PROFILE = ShopeeConst.API_GET_SHOP_PROFILE;

        private readonly ShareAppService _shareAppService;
        public ShopAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<string> GetShopInfo(long shop_id, string access_token)
        {
            var url = _shareAppService.GetShopeeUrl(API_GET_SHOP_INFO, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> GetShopProfile(int shop_id, string access_token)
        {
            var url = _shareAppService.GetShopeeUrl(API_GET_SHOP_PROFILE, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
