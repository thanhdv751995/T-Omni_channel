using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.Shops
{
    public interface IShopAppService : IApplicationService
    {
        Task<PagedResultDto<GShopDto>> GetListShop(string app, string client_id);

        Task UpdateLinkedShop(string channel_token);
        Task<ShopDetailDto> ShopDetail(string channel_token);
    }
}
