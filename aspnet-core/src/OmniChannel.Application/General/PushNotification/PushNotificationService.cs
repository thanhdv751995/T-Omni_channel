using OmniChannel.Channels;
using OmniChannel.General.Orders;
using OmniChannel.General.PushNotifications;
using OmniChannel.Orders;
using OmniChannel.TiktokShop.Orders;
using OmniChannel.Reverses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmniChannel.General.PushNotification
{
    /// <summary>
    /// Web hook
    /// </summary>
    public class PushNotificationService : OmniChannelAppService, IPushNotificationService
    {
        #region Const
        private readonly ReverseManager _reverseManager;
        private readonly OrderManager _orderManager;
        private readonly IOrdersAppService _orderAppService;
        private readonly IOrderRepository _orderRepository;
        private readonly IReverseRepository _reverseRepository;
        #endregion 

        public PushNotificationService(IOrderRepository orderRepository
            , IOrdersAppService orderAppService, IReverseRepository reverseRepository,
            ReverseManager reverseManager, OrderManager orderManager)
        {
            _reverseManager = reverseManager;
            _orderRepository = orderRepository;
            _orderAppService = orderAppService;
            _reverseRepository = reverseRepository;
            _orderManager = orderManager;
        }

        /// <summary>
        /// Cập nhật thông tin tạo đơn/hủy đơn/trạng thái vận chuyển
        /// </summary>
        /// <param name="order">Thông tin đơn hàng</param>
        /// <param name="chanel">Kênh bán hàng</param>
        /// <param name="channel_token">channel token</param>
        /// <returns></returns>
        public async Task<dynamic> OrderUpdate(TikTokNotificatio1nModel order, EChannel chanel, string channel_token)
       {
            // Lấy thông tin đơn hàng từ TikTok vừa mới thay đổi
            var tiktokOrder = await _orderAppService.OrderDetail(new RequestOrderIdList { order_id_list = { order.Data.Order_id } }, channel_token);
            // Nếu đơn hàng không tồn tại => trả vể null
            if (tiktokOrder == null || tiktokOrder.Data == null || tiktokOrder.Data.order_list == null) return null;
            var tiktokOrderInfo = tiktokOrder.Data.order_list[0];

            // Thông tin đơn hàng
            Order currentOrder = await _orderRepository.FindAsync(x => x.EChannel == chanel && x.OrderId == order.Data.Order_id && x.ShopId == order.Shop_id);

            // Chuyển đổi thông tin để có trạng thái đơn hàng tương ứng
            switch (order.Data.Order_status)
            {
                // Chưa thanh toán
                case OmniChannelConsts.PUSH_NOTIFICATION_ORDER_STATUS.UNPAID:
                    // Nếu đơn hàng không tồn tại trả về null
                    if (currentOrder != null) return currentOrder;

                    var itemLst = new List<Orders.ItemOrderDetailDto>();
                    // Duyệt danh sách sản phẩm trong đơn hàng
                    tiktokOrderInfo.item_list.ForEach(item =>
                    {
                        // Tạo danh sách sản phẩm của đơn hàng hiện tại
                        itemLst.Add(new Orders.ItemOrderDetailDto
                        {
                            // Mã sản phẩm
                            Product_id = item.product_id,
                            // Tên sản phẩm
                            Product_name = item.product_name,
                            // Số lượng
                            Quantity = item.quantity,
                            // Mã sku
                            Sku_id = item.sku_id,
                            // Đơn giá gốc
                            Sku_original_price = item.sku_original_price,
                            // Giảm giá từ nhà bán hàng
                            Sku_seller_discount = item.sku_seller_discount,
                            // Giá bán
                            Sku_sale_price = item.sku_sale_price
                        });
                    });

                    // Tạo thông tin đơn hàng
                    GOrdersDto orderModel = new()
                    {
                        // Kênh bán hàng
                        E_channel = chanel,
                        // Mã cửa hàng
                        Shop_id = order.Shop_id,
                        // Mã đơn hàng
                        Order_id = order.Data.Order_id,
                        // Trạng thái đơn hàng
                        Order_status = (int)OmniChannelConsts.ORDER_STATUS.UNPAID,
                        // Ngày cập nhật
                        Update_time = order.Data.Update_time,
                        // Mã khách hàng
                        Buyer_uid = tiktokOrderInfo.buyer_uid,
                        // Chi tiết đơn hàng
                        Order_detail = new GOrderDetailDto
                        {
                            // Lời nhắn của người mua hàng
                            Buyer_message = tiktokOrderInfo.buyer_message,
                            // Lý do hủy đơn hàng
                            Cancel_reason = tiktokOrderInfo.cancel_reason,
                            // Người hủy đơn
                            Cancel_user = tiktokOrderInfo.cancel_user,
                            // Ngày tạo đơn hàng
                            Create_time = tiktokOrderInfo.create_time,
                            // Nhà cung cấp dịch vụ vận chuyển
                            Shipping_provider = tiktokOrderInfo.shipping_provider,
                            // Mã nhà cung cấp dịch vụ vận chuyển
                            Shipping_provider_id = tiktokOrderInfo.shipping_provider_id,
                            // Mã vận đơn
                            Tracking_number = tiktokOrderInfo.tracking_number,
                            // Phương thức thanh toán
                            Payment_method = tiktokOrderInfo.payment_method,
                            // Thông tin thanh toán
                            Payment_info = new Orders.PaymentInfoOrderDetailDto
                            {
                                // Phí vận chuyển gốc
                                Original_shipping_fee = tiktokOrderInfo.payment_info.original_shipping_fee,
                                // Tổng giá gốc sản phẩm
                                Original_total_product_price = tiktokOrderInfo.payment_info.original_total_product_price,
                                // Phí vận chuyển
                                Shipping_fee = tiktokOrderInfo.payment_info.shipping_fee,
                                // Giảm giá vận chuyển từ sàn thương mại điện tử
                                Shipping_fee_platform_discount = tiktokOrderInfo.payment_info.shipping_fee_platform_discount,
                                // Giảm phí vận chuyển từ nhà bán hàng
                                Shipping_fee_seller_discount = tiktokOrderInfo.payment_info.shipping_fee_seller_discount,
                                // Tổng chi phí chưa có thuế và chiết khấu
                                Total_amount = tiktokOrderInfo.payment_info.total_amount,
                                // Tổng chi phí sau khi chiết khấu và thuế
                                Sub_total = tiktokOrderInfo.payment_info.sub_total,
                                // VAT
                                Taxes = tiktokOrderInfo.payment_info.taxes
                            },
                            // Danh sách sản phẩm của đơn hàng
                            Item_list = itemLst
                        }
                    };

                    // Khởi tạo thông tin đơn hàng
                    var newOrder = _orderManager.CreateAsync(orderModel);
                    // Thêm thông tin đơn hàng
                    return await _orderRepository.InsertAsync(newOrder);

                // Chờ giao hàng
                case OmniChannelConsts.PUSH_NOTIFICATION_ORDER_STATUS.AWAITING_SHIPMENT:
                    // Nếu đơn hàng không tồn tại trả về null
                    if (currentOrder == null) return null;

                    // Thời gian cập nhật đơn hàng
                    currentOrder.UpdateTime = order.Data.Update_time;
                    // Kênh bán hàng
                    currentOrder.EChannel = chanel;
                    // Cập nhật trạng thái đơn hàng
                    currentOrder.OrderStatus = (int)OmniChannelConsts.ORDER_STATUS.AWAITING_SHIPMENT;
                    // Cập nhật thông tin đơn hàng
                    return await _orderRepository.UpdateAsync(currentOrder);

                // Đã hủy đơn hàng
                case OmniChannelConsts.PUSH_NOTIFICATION_ORDER_STATUS.CANCEL:
                    // Nếu đơn hàng không tồn tại trả về null
                    if (currentOrder == null) return null;

                    // Lý do hủy đơn
                    currentOrder.OrderDetails.Cancel_reason = tiktokOrderInfo.cancel_reason;
                    // Người hủy đơn
                    currentOrder.OrderDetails.Cancel_user = tiktokOrderInfo.cancel_user;
                    // Lời nhắn của người mua
                    currentOrder.OrderDetails.Buyer_message = tiktokOrderInfo.buyer_message;
                    // Thời gian tạo
                    currentOrder.OrderDetails.Create_time = tiktokOrderInfo.create_time;
                    // Thời gian cập nhật đơn hàng
                    currentOrder.UpdateTime = order.Data.Update_time;
                    // Kênh bán hàng
                    currentOrder.EChannel = chanel;
                    // Cập nhật trạng thái đơn hàng
                    currentOrder.OrderStatus = (int)OmniChannelConsts.ORDER_STATUS.CANCELLED;
                    // Cập nhật thông tin đơn hàng
                    return await _orderRepository.UpdateAsync(currentOrder);
                default: return null;
            }
        }

        /// <summary>
        /// Trả hàng / hoàn tiền
        /// </summary>
        /// <param name="order">Thông tin đơn hàng</param>
        /// <param name="chanel">Kênh bán hàng</param>
        /// <param name="channel_token">channel token</param>
        /// <returns></returns>
        public async Task<dynamic> ReverseOrderUpdate(TikTokNotificatio2nModel order, EChannel chanel, string channel_token)
        {
            // Lấy thông tin đơn hàng hiện tại
            var currentOrder = await _orderRepository.FindAsync(x => x.EChannel == chanel && x.OrderId == order.Data.Order_id && x.ShopId == order.Shop_id);
            // Nếu đơn hàng không tồn tại trả về null
            if (currentOrder == null) return null;

            // Lấy thông tin đơn hàng từ TikTok vừa mới thay đổi
            var tiktokOrder = await _orderAppService.OrderDetail(new RequestOrderIdList { order_id_list = { order.Data.Order_id } }, channel_token);
            // nếu đơn hàng không tồn tại => trả vể null
            if (tiktokOrder == null || tiktokOrder.Data == null || tiktokOrder.Data.order_list == null) return null;
            var orderInfo = tiktokOrder.Data.order_list[0];

            // Tạo thông tin đơn hàng trả
            var reverse = _reverseManager.CreateAsync(new Reverses.GReverseDto
            {
                // Kênh bán hàng
                E_channel = chanel,
                // Mã đơn hàng
                Order_id = currentOrder.OrderId,
                // Mã cửa hàng
                Shop_id = currentOrder.ShopId,
                // Tổng tiền hoàn lại
                Refund_total = orderInfo.payment_info.total_amount,
                // Lý do trả hàng
                Return_reason = orderInfo.cancel_reason,
                // Mã vận đơn(trả hàng)
                Return_tracking_id = orderInfo.tracking_number,
                // Mã đơn hàng trả
                Reverse_order_id = order.Data.Reverse_order_id,
                // Thời gian yêu cầu trả hàng
                Reverse_request_time = order.Timestamp,
                // Trang thái trả hàng
                Reverse_status_value = 0,
                // Cập nhật thời gian trả hàng
                Reverse_update_time = order.Data.Update_time,
                // Loại trả hàng
                Reverse_type = (int)Enum.Parse(typeof(OmniChannelConsts.REVERSE_TYPE), order.Data.Reverse_event_type)
            });

            // Thêm thông tin đơn hàng trả
            return await _reverseRepository.InsertAsync(reverse);
        }

        /// <summary>
        /// Cập nhật thông tin nhận hàng
        /// </summary>
        /// <param name="order">Thông tin đơn hàng</param>
        /// <param name="chanel">Kênh bán hàng</param>
        /// <param name="channel_token">channel token</param>
        /// <returns></returns>
        public async Task<dynamic> RecipientAddressUpdate(TikTokNotificatio3nModel order, EChannel chanel, string channel_token)
        {
            // Lấy thông tin đơn hàng từ TikTok vừa mới thay đổi
            var tiktokOrder = await _orderAppService.OrderDetail(new RequestOrderIdList { order_id_list = { order.Data.Order_id } }, channel_token);
            // Nếu đơn hàng không tồn tại => trả vể null
            if (tiktokOrder == null || tiktokOrder.Data == null || tiktokOrder.Data.order_list == null) return null;
            var orderInfo = tiktokOrder.Data.order_list[0];

            // Lấy thông tin đơn hàng hiện tại
            var currentOrder = await _orderRepository.FindAsync(x => x.OrderId == order.Data.Order_id && x.ShopId == order.Shop_id);
            // Nếu đơn hàng không tồn tại trả về null
            if (currentOrder == null) return null;

            // Người nhận cập nhật địa chỉ
            currentOrder.OrderDetails.receiver_address_updated = orderInfo.receiver_address_updated;
            // Địa chỉ nhận hàng
            currentOrder.OrderDetails.Recipient_address.Full_address = orderInfo.recipient_address.full_address;
            // Họ tên người nhận hàng
            currentOrder.OrderDetails.Recipient_address.Name = orderInfo.recipient_address.name;
            // Số điện thoại nhận hàng
            currentOrder.OrderDetails.Recipient_address.Phone = orderInfo.recipient_address.phone;
            // Thời gian cập nhật đơn hàng
            currentOrder.UpdateTime = order.Data.Update_time;
            // Cập nhật đơn hàng
            return await _orderRepository.UpdateAsync(currentOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<dynamic> PackageUpdate(TikTokNotificatio4nModel order)
        {
            // Lấy thông tin đơn hàng hiện tại
            var currentOrder = await _orderRepository.FindAsync(x => x.ShopId == order.Shop_id);
            // Nếu đơn hàng không tồn tại trả về null
            if (currentOrder == null) return null;
            // Cập nhật trạng thái đơn hàng
            //currentOrder.OrderDetails.
            // Thời gian cập nhật đơn hàng
            //currentOrder.UpdateTime = order.data.update_time;
            return await _orderRepository.UpdateAsync(currentOrder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<dynamic> ProductAuditResultUpdate(TikTokNotificatio5nModel order)
        {
            throw new NotImplementedException();
        }
    }
}
