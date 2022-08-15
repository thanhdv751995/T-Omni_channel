using OmniChannel.Channels;
using OmniChannel.FulFillments.Package;
using OmniChannel.Orders;
using OmniChannel.TiktokShop.Orders;
using System.Threading.Tasks;

namespace OmniChannel.General.Orders
{
    /// <summary>
    /// Kế thừa toàn bộ các method
    /// </summary>
    public interface IGOrdersAppService : IOrdersAppService
    {
        #region TPOS

        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns> 
        Task<dynamic> GetOrderList(EChannel channel, string shopId, int orderStatus, long? start_time, long? end_time);

        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns> 
        Task<dynamic> GetOrderById(EChannel channel, string shopId, string orderId);
        #endregion

        /// <summary>
        /// Đồng bộ đơn hàng
        /// </summary>
        /// <returns></returns>
        Task OrderListSynchronized(EChannel eChannel);

        /// <summary>
        /// Cập nhật đơn hàng TPOS
        /// </summary>
        /// <param name="oldOrder"></param>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        Task UpdateOrder(OrderDetailDto newOrder);

        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="requestShipPackageDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<string> ShipPackage(RequestShipPackageDto requestShipPackageDto, string channel_token);
    }
}
