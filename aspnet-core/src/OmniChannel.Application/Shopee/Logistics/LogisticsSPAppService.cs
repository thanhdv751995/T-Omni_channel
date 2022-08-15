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

namespace OmniChannel.Shopee.Logistics
{
    [RemoteService(false)]
    public class LogisticsSPAppService : OmniChannelAppService
    {
        private readonly string API_GET_CHANNEL_LIST = ShopeeConst.API_GET_CHANNEL_LIST;

        private readonly ShareAppService _shareAppService;

        public LogisticsSPAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<ResponseDataSPDto<ResponseLogisticsChannelListDto>> GetChannels(long shop_id, string access_token)
        {
            var url = _shareAppService.GetShopeeUrl(API_GET_CHANNEL_LIST, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseLogisticsChannelListDto>>(httpResponseMessage);
        }
    }
}
