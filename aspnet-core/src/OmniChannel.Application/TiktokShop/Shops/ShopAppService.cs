using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OmniChannel.ChannelAuthentications;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using OmniChannel.TiktokShop.Shops;
using Volo.Abp;

namespace OmniChannel.Shops
{
    [RemoteService(true)]
    public class ShopAppService : OmniChannelAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly string TiktokShopApiGetAuthorizedShop = OmniChannelConsts.TiktokShopApiGetAuthorizedShop;
        //private readonly string shopId = "VNLC2YWLCF";
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly IConfiguration _configuration;

        public ShopAppService(ShareAppService shareAppService , IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }

        /// <summary>
        /// Get authoied shop
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseShopAuthorizeListDto>> GetAuthorizedShop(string access_token)
        {
            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetAuthorizedShop, _configuration["AppTiktokShopSetting:App_key"], access_token,
                "", _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseShopAuthorizeListDto>>(httpResponseMessage);
        }
    }
}
