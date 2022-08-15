using OmniChannel.Orders;
using OmniChannel.Reverses;
using OmniChannel.Shares;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.TiktokShop.Reverses
{
    public interface IReverseOrderAppService : IApplicationService
    {
        /// <summary>
        /// Xác nhận trả hàng
        /// </summary>
        /// <param name="requestConfirmRequestDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<string> ConfirmReverseRequest(RequestConfirmRequestDto requestConfirmRequestDto, string channel_token);

        /// <summary>
        /// Từ chối trả hàng
        /// </summary>
        /// <param name="requestRejectRequestDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<string> RejectReverseRequest(RequestRejectReverseRequestDto requestRejectRequestDto, string channel_token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCancelOrderDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<string> CancelOrder(RequestCancelOrderDto requestCancelOrderDto, string channel_token);

        /// <summary>
        /// Lấy danh sách các lý do hủy/trả hàng
        /// </summary>
        /// <param name="reverse_action_type"></param>
        /// <param name="reason_type"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<ResponseDataDto<ResponseReverseReasonDto>> GetRejectReasonList(int? reverse_action_type, int? reason_type, string channel_token);
    }
}
