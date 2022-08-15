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

namespace OmniChannel.Shopee.Attributes
{
    [RemoteService(false)]
    public class AttributeSPAppService : OmniChannelAppService
    {
        private readonly string LANGUAGE = ShopeeConst.LANGUAGE;
        private readonly string API_GET_ATTRIBUTES = ShopeeConst.API_GET_ATTRIBUTES;

        private readonly ShareAppService _shareAppService;
        public AttributeSPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<ResponseDataSPDto<ResponseDataListAttributeSPDto>> GetSPAttributes(long shop_id, string access_token, string category_id)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"language={LANGUAGE}", $"category_id={category_id}");

            var url = _shareAppService.GetShopeeUrl(API_GET_ATTRIBUTES, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseDataListAttributeSPDto>>(httpResponseMessage);
        }
    }
}
