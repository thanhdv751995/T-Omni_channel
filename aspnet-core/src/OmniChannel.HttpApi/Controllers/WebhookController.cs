using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmniChannel.Channels;
using OmniChannel.General.Kafka;
using OmniChannel.General.PushNotifications;
using System;
using System.Threading.Tasks;

namespace OmniChannel.Controllers
{
    [Route("api/webhook")]
    public class WebhookController : OmniChannelController
    {
        private readonly IPushNotificationService _notificationService;
        private readonly ApacheKafkaProducerService _kafkaService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="pushNotificationService"></param>
        public WebhookController(IPushNotificationService pushNotificationService, ApacheKafkaProducerService kafkaService)
        {
            _notificationService = pushNotificationService;
            _kafkaService = kafkaService;
        }

        /// <summary>
        /// Nhận thông báo từ TikTok shop
        /// </summary>
        /// <param name="data">Nội dung thông báo</param>
        /// <returns></returns>
        [HttpPost("notify-from-tiktok-shop")]
        public async Task<IActionResult> NotifyFromTiktokShop([FromBody] Object data)
        {
            if (data == null) return Ok();

            // TODO: Gán tạm chanel token
            Request.Headers.Add("ChannelAuthentication", "db30ce687f9c517c21ff6b1eefe8afc6350cef42863e0110ee9af6eda4101e51");
            // Lấy thông tin token
            var channel_token = Request.Headers["ChannelAuthentication"];
            // Chuyển đổi thông tin dữ liệu sau khi nhận push notification từ TikTok
            var tiktokData = JsonConvert.DeserializeObject<dynamic>(data.ToString());
            switch ((int)tiktokData.type)
            {
                case (int)OmniChannelConsts.PUSH_NOTIFICATION_STATUS.ORDER_STATUS_UPDATE:
                    TikTokNotificatio1nModel data1 = new TikTokNotificatio1nModel()
                    {
                        // Loại trạng thái thông báo
                        Push_type = tiktokData.type,
                        // Mã cửa hàng
                        Shop_id = tiktokData.shop_id,
                        // Thời gian
                        Timestamp = tiktokData.timestamp,
                        Data = new DataType1()
                        {
                            // Mã đơn hàng
                            Order_id = tiktokData.data.order_id,
                            // Trạng thái đơn hàng
                            Order_status = tiktokData.data.order_status,
                            // Thời gian cập nhật
                            Update_time = tiktokData.data.update_time
                        }
                    };
                    // Cập nhật thông tin đơn hàng
                    var retOrderUpdate = await OrderUpdate(data1, EChannel.TiktokShop, channel_token);
                    // Trả về 401 nếu thao tác trả hàng có lỗi
                    if (retOrderUpdate == null) return Unauthorized();
                    // Gửi thông báo tới client nếu cập nhật đơn hàng thành công
                    _kafkaService.Post(JsonConvert.SerializeObject(retOrderUpdate));
                    break;
                case (int)OmniChannelConsts.PUSH_NOTIFICATION_STATUS.REVERSE_ORDER:
                    TikTokNotificatio2nModel data2 = new TikTokNotificatio2nModel()
                    {
                        // Loại trạng thái thông báo
                        Push_type = tiktokData.type,
                        // Mã cửa hàng
                        Shop_id = tiktokData.shop_id,
                        // Thời gian
                        Timestamp = tiktokData.timestamp,
                        // Khởi tạo thông tin trả hàng / hoàn tiền
                        Data = new DataType2()
                        {
                            // Mã đơn hàng
                            Order_id = tiktokData.data.order_id,
                            // Loại trả hàng
                            Reverse_event_type = tiktokData.data.reverse_event_type,
                            // Mả đơn hàng trả
                            Reverse_order_id = tiktokData.data.reverse_order_id,
                            // Thời gian cập nhật
                            Update_time = tiktokData.data.update_time
                        }
                    };

                    // Thêm thông tin đơn hàng trả
                    var retReverseOrder = await ReverseOrderUpdate(data2, EChannel.TiktokShop, channel_token);
                    // Trả về 401 nếu thao tác trả hàng có lỗi
                    if (retReverseOrder == null) return Unauthorized();
                    // Gửi thông báo tới client nếu trả hàng thành công
                    _kafkaService.Post(JsonConvert.SerializeObject(retReverseOrder));
                    break;
                case (int)OmniChannelConsts.PUSH_NOTIFICATION_STATUS.RECEIVER_ADDRESS_UPDATED:
                    TikTokNotificatio3nModel data3 = new TikTokNotificatio3nModel()
                    {
                        // Loại trạng thái thông báo
                        Push_type = tiktokData.type,
                        // Mã cửa hàng
                        Shop_id = tiktokData.shop_id,
                        // Thời gian
                        Timestamp = tiktokData.timestamp,
                        Data = new DataType3()
                        {
                            // mã đơn hàng
                            Order_id = tiktokData.data.order_id,
                            // Thời gian cập nhật
                            Update_time = tiktokData.data.update_time
                        }
                    };

                    // Thêm thông tin đơn hàng trả
                    var retAddressUpdate = await RecipientAddressUpdate(data3, EChannel.TiktokShop, channel_token);
                    // Trả về 401 nếu thao tác có lỗi
                    if (retAddressUpdate == null) return Unauthorized();
                    // Gửi thông báo tới client nếu cập nhật thành công
                    _kafkaService.Post(JsonConvert.SerializeObject(retAddressUpdate));
                    break;
                case (int)OmniChannelConsts.PUSH_NOTIFICATION_STATUS.PACKAGE_UPDATED:
                    TikTokNotificatio4nModel data4 = new TikTokNotificatio4nModel()
                    {
                        // Loại trạng thái thông báo
                        Push_type = tiktokData.type,
                        // Mã cửa hàng
                        Shop_id = tiktokData.shop_id,
                        // Thời gian
                        Timestamp = tiktokData.timestamp,
                        Data = new DataType4()
                        {
                            //
                            Sc_type = tiktokData.sc_type,
                            // Quyền hạn
                            Role_type = tiktokData.tole_type,
                            // Danh mục hàng hóa
                            Package_list = tiktokData.package_list,
                            // Thời gian cập nhật
                            Update_time = tiktokData.data.update_time
                        }
                    };

                    // Thêm thông tin gói hàng
                    var retPackageUpdate = await PackageUpdate(data4);
                    // Trả về 401 nếu thao tác có lỗi
                    if (retPackageUpdate == null) return Unauthorized();
                    // Gửi thông báo tới client nếu cập nhật thành công
                    _kafkaService.Post(JsonConvert.SerializeObject(retPackageUpdate));
                    break;
                case (int)OmniChannelConsts.PUSH_NOTIFICATION_STATUS.PRODUCT_AUDIT_RESULT_UPDATE:
                    TikTokNotificatio5nModel data5 = new TikTokNotificatio5nModel()
                    {
                        // Loại trạng thái thông báo
                        Push_type = tiktokData.type,
                        // Mã cửa hàng
                        Shop_id = tiktokData.shop_id,
                        // Thời gian
                        Timestamp = tiktokData.timestamp,
                        // Khởi tạo thông tin sản phẩm
                        Data = new DataType5()
                        {
                            // Mã sản phẩm
                            Product_id = tiktokData.product_id,
                            // Trạng thái
                            Status = tiktokData.status,
                            // Thời gian cập nhật
                            Update_time = tiktokData.data.update_time
                        }
                    };
                    break;
                default:
                    break;
            }

            return Ok();
        }

        #region Private: Xử lý thông tin khi nhận thông báo
        /// <summary>
        /// Cập nhật thông tin đơn hàng khi có thông báo
        /// </summary>
        /// <param name="data">Dữ liệu đơn hàng</param>
        private async Task<dynamic> OrderUpdate(TikTokNotificatio1nModel data, EChannel chanel, string channel_token)
        {
            // Thực hiên cập nhật trạng thái đơn hàng vào MongoDB
            return await _notificationService.OrderUpdate(data, chanel, channel_token);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng khi có thông báo trả hàng/hoàn tiền
        /// </summary>
        /// <param name="data">Dữ liệu cầ cập nhật</param>
        private async Task<dynamic> ReverseOrderUpdate(TikTokNotificatio2nModel data, EChannel chanel, string channel_token)
        {
            // Thực hiên cập nhật trạng thái đơn hàng vào MongoDB
            return await _notificationService.ReverseOrderUpdate(data, chanel, channel_token);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng khi có thông báo cập nhận địa chỉ nhận hàng mới
        /// </summary>
        /// <param name="data">Dữ liệu cầ cập nhật</param>
        private async Task<dynamic> RecipientAddressUpdate(TikTokNotificatio3nModel data, EChannel chanel, string channel_token)
        {
            // Thực hiên cập nhật trạng thái đơn hàng vào MongoDB
            return await _notificationService.RecipientAddressUpdate(data, chanel, channel_token);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng khi có thông báo cập nhật gói hàng
        /// </summary>
        /// <param name="data">Dữ liệu cần cập nhật</param>`
        private async Task<dynamic> PackageUpdate(TikTokNotificatio4nModel data)
        {
            // Thực hiên cập nhật trạng thái đơn hàng vào MongoDB
            return await _notificationService.PackageUpdate(data);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng khi có thông báo kết quả kiểm tra hàng hóa
        /// </summary>
        /// <param name="data">Dữ liệu cần cập nhật</param>
        private async Task<dynamic> ProductAuditResultUpdate(TikTokNotificatio5nModel data)
        {
            // Thực hiên cập nhật trạng thái đơn hàng vào MongoDB
            return await _notificationService.ProductAuditResultUpdate(data);
        }
        #endregion
    }
}
