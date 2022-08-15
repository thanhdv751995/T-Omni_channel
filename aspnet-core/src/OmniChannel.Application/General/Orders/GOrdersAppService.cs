using Hangfire;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.FulFillments.Package;
using OmniChannel.FulFillMents;
using OmniChannel.Orders;
using OmniChannel.Reverses;
using OmniChannel.Shares;
using OmniChannel.TiktokShop.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.Orders
{
    [RemoteService(false)]
    /// <summary>
    /// Order service
    /// </summary>
    public class GOrdersAppService : OmniChannelAppService, IGOrdersAppService
    {
        #region Const
        private readonly IOrdersAppService _orderAppService;
        private readonly OrderManager _orderManager;
        private readonly ReverseManager _reverseManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IReverseRepository _reverseRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly FulFillMentAppService _fulFillMentAppService;
        private readonly ReverseAppService _reverseAppService;
        private readonly ShareAppService _shareAppService;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        #endregion

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="orderAppService"></param>
        /// <param name="orderManager"></param>
        /// <param name="orderRepository"></param>
        public GOrdersAppService(
              IOrdersAppService orderAppService, 
              OrderManager orderManager,
              ReverseManager reverseManager,
              IOrderRepository orderRepository,
              IReverseRepository reverseRepository,
              IBackgroundJobClient backgroundJobClient , 
              FulFillMentAppService fulFillMentAppService, 
              ReverseAppService reverseAppService,
              ShareAppService shareAppService,
              ChannelAuthenticationAppService channelAuthenticationAppService)
        {
            _orderAppService = orderAppService;
            _orderManager = orderManager;
            _reverseManager = reverseManager;
            _orderRepository = orderRepository;
            _reverseRepository = reverseRepository;
            _backgroundJobClient = backgroundJobClient;
            _fulFillMentAppService = fulFillMentAppService;
            _reverseAppService = reverseAppService;
            _shareAppService = shareAppService;
            _channelAuthenticationAppService = channelAuthenticationAppService;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="searchListOrderDto">Danh sách mã đơn hàng</param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseOderListDto>> Orders(RequestSearchListOrderDto searchListOrderDto, string channel_token)
        {
            // Danh sách đơn hàng từ TikTok shop
            return await _orderAppService.Orders(searchListOrderDto, channel_token);
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="requestOrderIdList">Danh sách mã đơn hàng</param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseOrderDetailListDto>> OrderDetail(RequestOrderIdList requestOrderIdList, string channel_token)
        {
            // Lấy thông tin chi tiết đơn hàng dựa vào mã đơn hàng
            return await _orderAppService.OrderDetail(requestOrderIdList, channel_token);
        }

        /// <summary>
        /// Đồng bộ đơn hàng
        /// </summary>
        /// <returns></returns>
        public async Task OrderListSynchronized(EChannel eChannel)
        {
            var listChannel = await _channelAuthenticationAppService.GetListChannelTiktokShop();
            if (listChannel != null)
            {
                foreach (var channel in listChannel)
                {
                    if (channel != null)
                    {
                        RequestSearchListOrderDto searchListProductDto = new()
                        {
                            // Số lượng đơn hàng trên 1 trang
                            Page_size = 20,
                            // Thông tin dùng để tìm kiếm những đơn hàng cho trang tiếp theo
                            Cursor = ""
                        };

                        // Get order id list
                        ResponseDataDto<ResponseOderListDto> listOrder = null;
                        do
                        {
                            // Lấy danh sách thông tin đơn hàng
                            listOrder = await _orderAppService.Orders(searchListProductDto, channel.Channel_token);

                            var orderIdLst = new RequestOrderIdList();
                            // Duyệt danh sách đơn hàng
                            listOrder.Data.order_list.ForEach(x =>
                            {
                                // Tạo danh sách mã đơn hàng
                                orderIdLst.order_id_list.Add(x.order_id);
                            });

                            // Lấy thông tin chi tiết đơn hàng theo danh sách mã đơn hàng
                            var orders = await _orderAppService.OrderDetail(orderIdLst, channel.Channel_token);
                            // Duyệt danh sách đơn hàng
                            foreach (var order in orders.Data.order_list)
                            {
                                // Kiểm tra đơn hàng đã được đồng bộ trước đó hay chưa
                                var orderInfo = await _orderRepository.FindAsync(x => x.EChannel == eChannel && x.ShopId == channel.Shop_id && x.OrderId == order.order_id);

                                // Đơn hàng đã được đồng bộ trước đó
                                if (orderInfo != null)
                                {
                                    // Chỉ cập nhật những đơn hàng chưa hoàn thành hoặc chưa hủy đơn hàng hoặc chưa giao hàng thành công
                                    if (orderInfo.OrderStatus <= (int)OmniChannelConsts.ORDER_STATUS.DELIVERED)
                                    {
                                        // Thực hiện cập nhật đơn hàng về TPos
                                        await Update(orderInfo, order);
                                    }
                                }
                                else
                                {
                                    // Khởi tạo danh sách các mặt hàng trong đơn hàng
                                    var itemLst = new List<ItemOrderDetailDto>();
                                    // Duyệt danh sách các mặt hàng trong đơn hàng
                                    order.item_list.ForEach(item =>
                                    {
                                        // Tạo danh sách sản phẩm của đơn hàng hiện tại
                                        itemLst.Add(new ItemOrderDetailDto
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
                                        // kênh bán hàng
                                        E_channel = EChannel.TiktokShop,
                                        // Mã cửa hàng
                                        Shop_id = channel.Shop_id,
                                        // Mã đơn hàng
                                        Order_id = order.order_id,
                                        // Trạng thái đơn hàng
                                        Order_status = order.order_status,
                                        // Ngày cập nhật
                                        Update_time = order.update_time,
                                        // Mã khách hàng
                                        Buyer_uid = order.buyer_uid,
                                        // Ngày bán
                                        Sale_date = order.create_time,
                                        // Chi tiết đơn hàng
                                        Order_detail = new GOrderDetailDto
                                        {
                                            // Lời nhắn của người mua hàng
                                            Buyer_message = order.buyer_message,
                                            // Lý do hủy đơn hàng
                                            Cancel_reason = order.cancel_reason,
                                            // Người hủy đơn
                                            Cancel_user = order.cancel_user,
                                            // Ngày tạo đơn hàng
                                            Create_time = order.create_time,
                                            // Nhà cung cấp dịch vụ vận chuyển
                                            Shipping_provider = order.shipping_provider,
                                            // mã nhà cung cấp dịch vụ vận chuyển
                                            Shipping_provider_id = order.shipping_provider_id,
                                            // Mã vận đơn
                                            Tracking_number = order.tracking_number,
                                            // Phương thức thanh toán
                                            Payment_method = order.payment_method,
                                            // Thời gian thanh toán
                                            Paid_time = order.paid_time,
                                            // Thông tin thanh toán
                                            Payment_info = new PaymentInfoOrderDetailDto
                                            {
                                                // Phí vận chuyển gốc
                                                Original_shipping_fee = order.payment_info.original_shipping_fee,
                                                // Tổng giá gốc sản phẩm
                                                Original_total_product_price = order.payment_info.original_total_product_price,
                                                // Phí vận chuyển
                                                Shipping_fee = order.payment_info.shipping_fee,
                                                // Giảm giá vận chuyển từ sàn thương mại điện tử
                                                Shipping_fee_platform_discount = order.payment_info.shipping_fee_platform_discount,
                                                // Giảm phí vận chuyển từ nhà bán hàng
                                                Shipping_fee_seller_discount = order.payment_info.shipping_fee_seller_discount,
                                                // Tổng chi phí chưa có thuế và chiết khấu
                                                Total_amount = order.payment_info.total_amount,
                                                // Tổng chi phí sau khi chiết khấu và thuế
                                                Sub_total = order.payment_info.sub_total,
                                                // VAT
                                                Taxes = order.payment_info.taxes
                                            },
                                            // Danh sách sản phẩm của đơn hàng
                                            Item_list = itemLst
                                        }
                                    };

                                    // Đồng bộ đơn hàng mới cho TPos
                                    await CreateOrder(orderModel);
                                }
                            }

                            searchListProductDto = new()
                            {
                                // Số lượng đơn hàng trên 1 trang
                                Page_size = 20,
                                // Thông tin dùng để tìm kiếm những đơn hàng cho trang tiếp theo
                                Cursor = listOrder.Data.next_cursor
                            };

                        } while (listOrder != null && listOrder.Data.more); // Nếu không còn đơn hàng trang tiếp theo thì dừng
                    }
                }
            }
        }

        #region TPOS
        /// <summary>
        /// Lấy danh sách đơn hàng cho TPos
        /// </summary>
        /// <param name="searchListOrderDto"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<dynamic> GetOrderList(EChannel channel, string shopId, int orderStatus, long? start_time, long? end_time)
        {
            // Lấy thông tin đơn hàng theo: trạng thái, và ngày tạo đơn hàng
            if (orderStatus != 0 && start_time != 0 && end_time != 0)
            {
                // Lấy thông tin đơn hàng theo: trạng thái, và ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderStatus == orderStatus
                                                            && x.OrderDetails.Create_time >= start_time && x.OrderDetails.Create_time <= end_time);
            }
            else if(orderStatus != 0 && start_time == 0 && end_time == 0)
            {
                // Lấy thông tin đơn hàng theo trạng thái
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderStatus == orderStatus);
            }
            else if (orderStatus != 0 && start_time != 0 && end_time == 0)
            {
                // Lấy thông tin đơn hàng theo ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderStatus == orderStatus 
                                                            && x.OrderDetails.Create_time >= start_time);
            }
            else if (orderStatus != 0 && start_time == 0 && end_time != 0)
            {
                // Lấy thông tin đơn hàng theo ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderStatus == orderStatus 
                                                            && x.OrderDetails.Create_time <= end_time);
            }
            else if (start_time != 0 && end_time != 0)
            {
                // Lấy thông tin đơn hàng theo ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderStatus == orderStatus 
                                                            && x.OrderDetails.Create_time >= start_time && x.OrderDetails.Create_time <= end_time);
            }
            else if(orderStatus == 0 && start_time != 0 && end_time == 0)
            {
                // Lấy thông tin đơn hàng theo ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderDetails.Create_time >= start_time);
            }
            else if (orderStatus == 0 && start_time == 0 && end_time != 0)
            {
                // Lấy thông tin đơn hàng theo ngày tạo đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderDetails.Create_time <= end_time);
            }
            else
            {
                // Lấy tất cả danh sách đơn hàng
                return await _orderRepository.GetListAsync(x => x.EChannel == channel && x.ShopId == shopId);
            }
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo mã đơn hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<dynamic> GetOrderById(EChannel channel, string shopId, string orderId)
        {
            return await _orderRepository.FindAsync(x => x.EChannel == channel && x.ShopId == shopId && x.OrderId == orderId);
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="shopId"></param>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public async Task UpdateOrder(OrderDetailDto newOrder)
        {
            var currentOrder = await _orderRepository.FindAsync(x => x.OrderId == newOrder.order_id);
            // Thực hiện cập nhật đơn hàng về TPos
            await Update(currentOrder, newOrder);
        }

        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="orderDto">Thông tin đơn hàng</param>
        /// <returns></returns>
        private async Task<dynamic> CreateOrder(GOrdersDto orderDto)
        {
            // Tạo đơn hàng
            var order = _orderManager.CreateAsync(orderDto);
            return await _orderRepository.InsertAsync(order);
        }


        /// <summary>
        /// Cập nhật thông tin đơn hàng
        /// </summary>
        /// <param name="createGProductDto"></param>
        private async Task Update(Order oldOrder, OrderDetailDto newOrder)
        {
            // Khởi tạo danh sách các mặt hàng trong đơn hàng
            var itemLst = new List<ItemOrderDetailDto>();
            // Duyệt danh sách các mặt hàng trong đơn hàng
            newOrder.item_list.ForEach(item =>
            {
                // Tạo danh sách sản phẩm của đơn hàng hiện tại
                itemLst.Add(new ItemOrderDetailDto
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

            // Trạng thái đơn hàng
            oldOrder.OrderStatus = newOrder.order_status;
            // Ngày cập nhật đơn hàng
            oldOrder.UpdateTime = newOrder.update_time;
            // Mã khách hàng
            oldOrder.BuyerId = newOrder.buyer_uid;
            // Ngày bán
            oldOrder.SaleDate = newOrder.create_time;
            // Chi tiết đơn hàng
            oldOrder.OrderDetails = new GOrderDetailDto
            {
                // Lời nhắn của người mua hàng
                Buyer_message = newOrder.buyer_message,
                // Lý do hủy đơn hàng
                Cancel_reason = newOrder.cancel_reason,
                // Người hủy đơn
                Cancel_user = newOrder.cancel_user,
                // Ngày tạo đơn hàng
                Create_time = newOrder.create_time,
                // Nhà cung cấp dịch vụ vận chuyển
                Shipping_provider = newOrder.shipping_provider,
                // mã nhà cung cấp dịch vụ vận chuyển
                Shipping_provider_id = newOrder.shipping_provider_id,
                // Mã vận đơn
                Tracking_number = newOrder.tracking_number,
                // Phương thức thanh toán
                Payment_method = newOrder.payment_method,
                // Thời gian thanh toán
                Paid_time = newOrder.paid_time,
                // Thông tin thanh toán
                Payment_info = new PaymentInfoOrderDetailDto
                {
                    // Phí vận chuyển gốc
                    Original_shipping_fee = newOrder.payment_info.original_shipping_fee,
                    // Tổng giá gốc sản phẩm
                    Original_total_product_price = newOrder.payment_info.original_total_product_price,
                    // Phí vận chuyển
                    Shipping_fee = newOrder.payment_info.shipping_fee,
                    // Giảm giá vận chuyển từ sàn thương mại điện tử
                    Shipping_fee_platform_discount = newOrder.payment_info.shipping_fee_platform_discount,
                    // Giảm phí vận chuyển từ nhà bán hàng
                    Shipping_fee_seller_discount = newOrder.payment_info.shipping_fee_seller_discount,
                    // Tổng chi phí chưa có thuế và chiết khấu
                    Total_amount = newOrder.payment_info.total_amount,
                    // Tổng chi phí sau khi chiết khấu và thuế
                    Sub_total = newOrder.payment_info.sub_total,
                    // VAT
                    Taxes = newOrder.payment_info.taxes
                },
                Item_list = itemLst
            };

            // Thực hiện cập nhật đơn hàng
            await _orderRepository.UpdateAsync(oldOrder);
        }

        ///// <summary>
        ///// Trả hàng
        ///// </summary>
        ///// <param name="orderDto"></param>
        ///// <returns></returns>
        ///// <exception cref="System.NotImplementedException"></exception>
        //public async Task<dynamic> ReverseOrder(RequestConfirmRequestDto requestReverseOrderDto, string channel_token)
        //{
        //    return await _reverseAppService.ConfirmReverseRequest(requestReverseOrderDto, channel_token);
        //}

        ///// <summary>
        ///// Hủy đơn hàng
        ///// </summary>
        ///// <param name="orderDto"></param>
        ///// <returns></returns>
        ///// <exception cref="System.NotImplementedException"></exception>
        //public async Task<dynamic> CancelOrder(RequestCancelOrderDto requestCancelOrderDto, string channel_token)
        //{
        //    //// Nếu đơn hàng được đặt dưới 1 giờ, khách hàng có thể hủy đơn mà không cần sự cho phép của người bán
        //    //var currentOrder = await _orderRepository.FindAsync(x => x.OrderId == requestCancelOrderDto.order_id);
        //    //if(currentOrder != null)
        //    //{
        //    //    // Nếu đơn hàng chưa thanh toán
        //    //    if (currentOrder.OrderStatus <= (int)OmniChannelConsts.ORDER_STATUS.AWAITING_SHIPMENT)
        //    //    {
        //    //        // Kiểm tra thời điểm đặt hàng so với thời gian yêu cầu hủy hàng có quá 1 tiếng hay không
        //    //        // Nếu nhỏ hơn 1 tiếng thì được hủy đơn hàng
        //    //        DateTime convertedUnixTime = DateTimeOffset.FromUnixTimeMilliseconds(currentOrder.OrderDetails.Create_time).DateTime;
        //    //        if (DateTime.UtcNow.Subtract(convertedUnixTime).TotalHours < 1)
        //    //        {
        //    //            return await _reverseAppService.CancelOrder(requestCancelOrderDto, channel_token);
        //    //        }
        //    //        else
        //    //        {
        //    //            return null;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        // Nếu đơn hàng đã thanh toán
        //    //        // Đơn hàng được thanh toán hơn 1 giờ vẫn có thể bị hủy trước khi vận chuyển
        //    //        // Tuy nhiên, phải được người bán chấp nhận
        //    //        if (currentOrder.OrderDetails.Payment_info != null)
        //    //        {
        //    //            DateTime ConvertedUnixTime = DateTimeOffset.FromUnixTimeMilliseconds(currentOrder.OrderDetails.Paid_time).DateTime;
        //    //            if (DateTime.UtcNow.Subtract(ConvertedUnixTime).TotalHours < 1
        //    //                && currentOrder.OrderStatus <= (int)OmniChannelConsts.ORDER_STATUS.AWAITING_SHIPMENT)
        //    //            {
        //    //                return await _reverseAppService.CancelOrder(requestCancelOrderDto, channel_token);
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            return null;
        //    //        }
        //    //    }

        //    //    //if(currentOrder)
        //    //}

        //    //return null;
        //    return await _reverseAppService.CancelOrder(requestCancelOrderDto, channel_token);
        //}

        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns> 
        public async Task<string> ShipPackage(RequestShipPackageDto requestShipPackageDto, string channel_token)
        {
            return await _fulFillMentAppService.ShipPackage(requestShipPackageDto, channel_token);
        }
        #endregion
    }
}
