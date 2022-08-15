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

namespace OmniChannel.Shopee.Models
{
    [RemoteService(false)]
    public class ModelSPAppService : OmniChannelAppService
    {
        private readonly string API_ADD_MODEL = ShopeeConst.API_ADD_MODEL;
        private readonly string API_UPDATE_MODEL = ShopeeConst.API_UPDATE_MODEL;
        private readonly string API_DELETE_MODEL = ShopeeConst.API_DELETE_MODEL;
        private readonly string API_GET_MODEL_LIST = ShopeeConst.API_GET_MODEL_LIST;

        private readonly ShareAppService _shareAppService;

        public ModelSPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<string> GetModels(long shop_id, string access_token, long item_id)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"item_id={item_id}");

            var url = _shareAppService.GetShopeeUrl(API_GET_MODEL_LIST, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<ResponseDataSPDto<ResponseAddModelDto>> AddModel(long shop_id, string access_token, [FromBody] AddModelSPDto addModelSPDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_ADD_MODEL, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, addModelSPDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseAddModelDto>>(httpResponseMessage);
        }

        [HttpPost]
        public async Task<ResponseDataSPDto<ResponseAddModelDto>> Delete(long shop_id, string access_token, [FromBody] DeleteModelDto deleteModelDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_DELETE_MODEL, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, deleteModelDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseAddModelDto>>(httpResponseMessage);
        }
    }
}
