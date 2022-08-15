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

namespace OmniChannel.Shopee.Categories
{
    [RemoteService(false)]
    public class CategorySPAppService : OmniChannelAppService
    {
        private readonly string API_GET_CATEGORY = ShopeeConst.API_GET_CATEGORY;
        private readonly string LANGUAGE = ShopeeConst.LANGUAGE;

        private readonly ShareAppService _shareAppService;
        public CategorySPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<ResponseDataSPDto<ResponseDataListCategorySPDto>> GetShopeeCategories(long shop_id, string access_token)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"language={LANGUAGE}");

            var url = _shareAppService.GetShopeeUrl(API_GET_CATEGORY, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseDataListCategorySPDto>>(httpResponseMessage);
        }
    }
}
