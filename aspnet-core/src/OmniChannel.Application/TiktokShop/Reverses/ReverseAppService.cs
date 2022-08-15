using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Orders;
using OmniChannel.Shares;
using OmniChannel.TiktokShop.Reverses;
using Volo.Abp;

namespace OmniChannel.Reverses
{
    [RemoteService(false)]
    public class ReverseAppService : OmniChannelAppService, IReverseOrderAppService
    {
        private readonly ShareAppService _shareAppService;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly string TiktokShopRejectReverseRequest = OmniChannelConsts.TiktokShopRejectReverseRequest;
        private readonly string TiktokShopGetReverseOrderList = OmniChannelConsts.TiktokShopGetReverseOrderList;
        private readonly string TiktokShopConfirmReverseRequest = OmniChannelConsts.TiktokShopConfirmReverseRequest;
        private readonly string TiktokShopCancelOrder = OmniChannelConsts.TiktokShopCancelOrder;
        private readonly string TiktokShopGetRejectReasonList = OmniChannelConsts.TiktokShopGetRejectReasonList;
        private readonly IConfiguration _configuration;
        public ReverseAppService(ShareAppService shareAppService , IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }

        public async Task<string> RejectReverseRequest(RequestRejectReverseRequestDto requestRejectReverseRequestDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopRejectReverseRequest, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestRejectReverseRequestDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<ResponseDataDto<ResponseGetReverseOrderListDto>> GetReverseOrder(RequestReverseOrderListDto requestReverseOrderListDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetReverseOrderList, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestReverseOrderListDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetReverseOrderListDto>>(httpResponseMessage);
        }

        public async Task<string> ConfirmReverseRequest(RequestConfirmRequestDto requestConfirmRequestDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopConfirmReverseRequest, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestConfirmRequestDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> CancelOrder(RequestCancelOrderDto requestCancelOrderDto , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopCancelOrder, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestCancelOrderDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<ResponseDataDto<ResponseReverseReasonDto>> GetRejectReasonList(int? reverse_action_type, int? reason_type, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"reverse_action_type={reverse_action_type}", $"reason_type={reason_type}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetRejectReasonList, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseReverseReasonDto>>(httpResponseMessage);
        }
    }
}
