using OmniChannel.Orders;
using OmniChannel.Shares;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.TiktokShop.Orders
{
    public interface IOrdersAppService : IApplicationService
    {
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="searchListOrderDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<ResponseDataDto<ResponseOderListDto>> Orders(RequestSearchListOrderDto searchListOrderDto, string channel_token);

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="requestOrderIdList"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        Task<ResponseDataDto<ResponseOrderDetailListDto>> OrderDetail(RequestOrderIdList requestOrderIdList, string channel_token);
    }
}
