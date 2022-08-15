using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using OmniChannel.Shopee.ResponseData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Shopee.Brands
{
    [RemoteService(false)]
    public class BrandSPAppService : OmniChannelAppService
    {
        private readonly string API_GET_BRAND_LIST = ShopeeConst.API_GET_BRAND_LIST;
        private readonly string LANGUAGE = ShopeeConst.LANGUAGE;

        private readonly ShareAppService _shareAppService;

        public BrandSPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        //1: normal brand, 2: pending brand
        public async Task<ResponseDataSPDto<ResponseGetBrandListSPDto>> GetBrands([Required] long shop_id, 
            [Required] string access_token, 
            [Required] string category_id,
            int offset = 0,
            int status = 1, 
            int page_size = 10)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"status={status}", 
                $"language={LANGUAGE}", 
                $"page_size={page_size}", 
                $"category_id={category_id}",
                $"offset={offset}");

            var url = _shareAppService.GetShopeeUrl(API_GET_BRAND_LIST, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseGetBrandListSPDto>>(httpResponseMessage);
        }
    }
}
