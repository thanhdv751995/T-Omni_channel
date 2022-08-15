using Microsoft.AspNetCore.Mvc;
using OmniChannel.General.Orders;
using OmniChannel.General.Reverses;
using OmniChannel.Orders;
using OmniChannel.Reverses;
using System.Threading.Tasks;

namespace OmniChannel.Controllers
{
    [Route("api/g-reverse")]
    public class ReverseController : OmniChannelController
    {
        private readonly IGReverseOrderAppService _reverseOrderService;
        private readonly IGOrdersAppService _orderService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="orderService"></param>
        public ReverseController(IGReverseOrderAppService reverseOrderService, IGOrdersAppService gOrdersAppService)
        {
            _reverseOrderService = reverseOrderService;
            _orderService = gOrdersAppService;
        }

        #region Call to Tiktok service

        /// <summary>
        /// Xác nhận trả đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpPost("get-reverse-order")]
        public async Task<dynamic> GetReverseOrder()
        {
            return await _reverseOrderService.GetReverseOrder();
        }

        /// <summary>
        /// Xác nhận trả đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpPost("confirm-reverse-request")]
        public async Task<string> ConfirmReverseRequest([FromBody] RequestConfirmRequestDto requestConfirmRequestDto)
        {
            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            // Thực hiện xác nhận trả hàng
            dynamic retData = await _reverseOrderService.ConfirmReverseRequest(requestConfirmRequestDto, channel_token);
            if(retData != null && retData.data != null)
            {
                // Lấy thông tin đơn hàng từ TikTok vừa mới thay đổi
                var tiktokOrder = await _orderService.OrderDetail(new RequestOrderIdList { order_id_list = { retData.data.reverse_main_order_id } }, channel_token);
                // nếu đơn hàng không tồn tại => trả vể null
                if (tiktokOrder == null || tiktokOrder.Data == null || tiktokOrder.Data.order_list == null) return null;
                var orderInfo = tiktokOrder.Data.order_list[0];

                // Cập nhật trạng thái đơn hàng
                await _orderService.UpdateOrder(orderInfo);
            }

            return retData;
        }

        /// <summary>
        /// Từ chối trả đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpPost("reject-reverse-request")]
        public async Task<string> RejectReverseRequest([FromBody] RequestRejectReverseRequestDto requestRejectRequestDto)
        {
            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            // Thực hiện xác nhận trả hàng
            dynamic retData = await _reverseOrderService.RejectReverseRequest(requestRejectRequestDto, channel_token);
            if (retData != null && retData.data != null)
            {
                // Lấy thông tin đơn hàng từ TikTok vừa mới thay đổi
                var tiktokOrder = await _orderService.OrderDetail(new RequestOrderIdList { order_id_list = { retData.data.reverse_main_order_id } }, channel_token);
                // nếu đơn hàng không tồn tại => trả vể null
                if (tiktokOrder == null || tiktokOrder.Data == null || tiktokOrder.Data.order_list == null) return null;
                var orderInfo = tiktokOrder.Data.order_list[0];

                // Cập nhật trạng thái đơn hàng
                await _orderService.UpdateOrder(orderInfo);
            }

            return retData;
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        [HttpPost("cancel-order")]
        public async Task<dynamic> CancelOrder([FromBody] RequestCancelOrderDto requestCancelOrderDto)
        {
            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            return await _reverseOrderService.CancelOrder(requestCancelOrderDto, channel_token);
        }

        /// <summary>
        /// Lấy danh sách lý do hủy đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        [HttpPost("get-reject-reason-list")]
        public async Task<dynamic> GetReasonList(int? reverse_action_type, int? reason_type)
        {
            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            return await _reverseOrderService.GetRejectReasonList(reverse_action_type, reason_type, channel_token);
        }
        #endregion
    }
}
