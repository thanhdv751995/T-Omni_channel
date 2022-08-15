using OmniChannel.General.Orders;
using System;
using Volo.Abp.Domain.Services;

namespace OmniChannel.Orders
{
    /// <summary>
    ///  Đơn hàng
    /// </summary>
    public class OrderManager : DomainService
    {
        public OrderManager()
        {
        }
        public Order CreateAsync(
              GOrdersDto order
           )
        {
            return new Order(
               GuidGenerator.Create(),
               order.Shop_id,
               order.Order_id,
               order.Update_time,
               order.Order_status,
               order.Buyer_uid,
               order.Sale_date,
               order.Order_detail
            );
        }
    }
}
