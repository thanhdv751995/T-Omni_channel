using OmniChannel.Channels;
using OmniChannel.General.Orders;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.Orders
{
    /// <summary>
    /// Đơn hàng
    /// </summary>
    public class Order : AuditedAggregateRoot<string>
    {
        /// <summary>
        /// Kênh bán hàng
        /// </summary>
        public EChannel EChannel { get; set; }

        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// Thời gian ...
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// Mã người mua hàng
        /// </summary>
        public string BuyerId { get; set; }

        /// <summary>
        /// Ngày bán
        /// </summary>
        public long SaleDate { get; set; }

        public GOrderDetailDto OrderDetails { get; set; }

        private Order()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal Order(
              Guid? id,
              string shop_id,
              string order_id,
              long update_time,
              int order_status,
              string buyer_uid,
              long sale_date,
              GOrderDetailDto order_detail
           )
           : base(id.ToString())
        {
            ShopId = shop_id;
            OrderId = order_id;
            OrderStatus = order_status;
            UpdateTime = update_time;
            BuyerId = buyer_uid;
            SaleDate = sale_date;
            OrderDetails = order_detail;
        }
    }
}
