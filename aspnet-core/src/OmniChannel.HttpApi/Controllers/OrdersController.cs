using Microsoft.AspNetCore.Mvc;
using OmniChannel.FulFillments.Package;
using OmniChannel.General;
using OmniChannel.General.Orders;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OmniChannel.Controllers
{
    /// <summary>
    /// Đơn hàng
    /// </summary>
    [Route("api/g-order")]
    public class OrdersController : OmniChannelController
    {
        private readonly IGOrdersAppService _orderService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="orderService"></param>
        public OrdersController(IGOrdersAppService orderService)
        {
            _orderService = orderService;
        }

        #region TPOS Api xử lý trên dữ liệu đã đồng bộ(MongoDB)
        /// <summary>
        /// Lấy thông tin danh sách đơn hàng
        /// </summary>
        /// <param name="requestOrderIdList"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpGet("get-order-list")]
        public async Task<dynamic> GetOrderList(SearchOrderByDateDto searchOrder)
        {
            // Không thể lấy thông tin dữ liệu đơn hàng
            var response = new GResponse<dynamic>
            {
                // Lấy dự liệu thất bại
                Success = false,
                // Dữ liệu đơn hàng trả về sẽ rỗng

                Data = null
            };

            try
            {
                long startTimestamp = 0;
                long endTimestamp = 0;
                if(searchOrder.Start_date > DateTime.MinValue)
                {
                    startTimestamp = new DateTimeOffset(searchOrder.Start_date).ToUnixTimeMilliseconds();
                }
                if (searchOrder.End_date > DateTime.MinValue)
                {
                    endTimestamp = new DateTimeOffset(searchOrder.End_date).ToUnixTimeMilliseconds();
                }

                // Lấy danh sách đơn hàng từ dữ liệu đã đồng bộ
                var results = await _orderService.GetOrderList(searchOrder.EChannel, searchOrder.Shop_id, searchOrder.Order_status, startTimestamp, endTimestamp);
                // Kiểm tra trạng thái 
                var statusCode = results == null ? HttpStatusCode.NoContent : HttpStatusCode.OK;
                // Tạo dữ liệu trả về
                response.Success = true;
                response.Data = results;

                // Trả về thông tin đơn hàng
                return SendResponse(response, statusCode);
            }
            catch
            {
                // Trả về thông báo lỗi
                return SendResponse(response, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng bằng order id
        /// </summary>
        /// <param name="requestOrderIdList"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpGet("get-order-by-id")]
        public async Task<dynamic> GetOrderById(SearchOrderByIdDto searchOrder)
        {
            // Không thể lấy thông tin dữ liệu đơn hàng
            var response = new GResponse<dynamic>
            {
                // Lấy dự liệu thất bại
                Success = false,
                // Dữ liệu đơn hàng trả về sẽ rỗng
                Data = null
            };

            try
            {
                // Lấy danh sách đơn hàng từ dữ liệu đã đồng bộ
                var results = await _orderService.GetOrderById(searchOrder.EChannel, searchOrder.Shop_id, searchOrder.Order_id);
                // Kiểm tra trạng thái 
                var statusCode = results == null ? HttpStatusCode.NoContent : HttpStatusCode.OK;
                // Tạo dữ liệu trả về
                response.Success = true;
                response.Data = results;

                // Trả về thông tin đơn hàng
                return SendResponse(response, statusCode);
            }
            catch
            {
                // Trả về thông báo lỗi
                return SendResponse(response, HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Call to Tiktok service
        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpPost("confirm-order")]
        public async Task<string> ShipPackage([FromBody] RequestShipPackageDto requestShipPackageDto)
        {
            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            return await _orderService.ShipPackage(requestShipPackageDto, channel_token);
        }
        #endregion
    }
}
