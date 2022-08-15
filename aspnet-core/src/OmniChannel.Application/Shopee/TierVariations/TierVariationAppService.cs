using Microsoft.AspNetCore.Mvc;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using OmniChannel.Shopee.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Shopee.TierVariations
{
    [RemoteService(false)]
    public class TierVariationAppService: OmniChannelAppService
    {
        private readonly string API_INIT_TIER_VARIATION = ShopeeConst.API_INIT_TIER_VARIATION;
        private readonly string API_UPDATE_TIER_VARIATION = ShopeeConst.API_UPDATE_TIER_VARIATION;

        private readonly ShareAppService _shareAppService;

        public TierVariationAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<ResponseDataSPDto<ResponseCreateTierVariationDto>> InitTierVariation(long shop_id, 
            string access_token, 
            [FromBody] RequestInitTierVariationDto requestInitTierVariationDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_INIT_TIER_VARIATION, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestInitTierVariationDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseCreateTierVariationDto>>(httpResponseMessage);
        }

        [HttpPost]
        public async Task<string> UpdateTierVariation(long shop_id, 
            string access_token, 
            [FromBody] RequestInitTierVariationDto requestInitTierVariationDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_UPDATE_TIER_VARIATION, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestInitTierVariationDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
