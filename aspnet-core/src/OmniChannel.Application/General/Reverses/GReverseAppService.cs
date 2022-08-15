using OmniChannel.Orders;
using OmniChannel.Reverses;
using OmniChannel.Shares;
using OmniChannel.TiktokShop.Reverses;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.Reverses
{
    [RemoteService(false)]
    public class GReverseAppService : OmniChannelAppService, IGReverseOrderAppService
    {
        private readonly IReverseOrderAppService _reverseAppService;
        private readonly IReverseRepository _reverseRepository;

        public GReverseAppService(IReverseOrderAppService reverseAppService, IReverseRepository reverseRepository)
        {
            _reverseAppService = reverseAppService;
            _reverseRepository = reverseRepository;
        }

        /// <summary>
        /// Xác nhận trả hàng
        /// </summary>
        /// <param name="requestConfirmRequestDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<string> ConfirmReverseRequest(RequestConfirmRequestDto requestConfirmRequestDto, string channel_token)
        {
            return await _reverseAppService.ConfirmReverseRequest(requestConfirmRequestDto, channel_token);
        }

        /// <summary>
        /// Từ chối trả hàng
        /// </summary>
        /// <param name="requestRejectRequestDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<string> RejectReverseRequest(RequestRejectReverseRequestDto requestRejectRequestDto, string channel_token)
        {
            return await _reverseAppService.RejectReverseRequest(requestRejectRequestDto, channel_token);
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="requestCancelOrderDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<string> CancelOrder(RequestCancelOrderDto requestCancelOrderDto, string channel_token)
        {
            return await _reverseAppService.CancelOrder(requestCancelOrderDto, channel_token);
        }

        /// <summary>
        /// Danh sách lý do hủy/trả hàng
        /// </summary>
        /// <param name="reverse_action_type"></param>
        /// <param name="reason_type"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseReverseReasonDto>> GetRejectReasonList(int? reverse_action_type, int? reason_type, string channel_token)
        {
            return await _reverseAppService.GetRejectReasonList(reverse_action_type, reason_type, channel_token);
        }

        /// <summary>
        /// Lấy danh sách trả hàng
        /// </summary>
        /// <param name="requestReverseOrderIdList"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<dynamic> GetReverseOrder()
        {
            return await _reverseRepository.GetListAsync();
        }
    }
}
