using Hangfire;
using Microsoft.Extensions.Configuration;
using OmniChannel.General.Orders;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using OmniChannel.TiktokShop.Orders;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Orders
{
    [RemoteService(false)]
    
    public class OrderAppService : OmniChannelAppService, IOrdersAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly string TiktokShopApiGetListOrder = OmniChannelConsts.TiktokShopAPIGetListOrder;
        private readonly string TiktokShopApiGetOrderDetail = OmniChannelConsts.TiktokShopApiGetOrderDetail;
        private readonly IConfiguration _configuration;

        public OrderAppService(ShareAppService shareAppService, IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="searchListOrderDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseOderListDto>> Orders(RequestSearchListOrderDto searchListOrderDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListOrder, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            HttpResponseMessage httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, searchListOrderDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseOderListDto>>(httpResponseMessage);
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="requestOrderIdList"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseOrderDetailListDto>> OrderDetail(RequestOrderIdList requestOrderIdList, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetOrderDetail, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            HttpResponseMessage httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestOrderIdList);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseOrderDetailListDto>>(httpResponseMessage);
        }
    }
}
