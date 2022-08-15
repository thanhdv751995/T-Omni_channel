using OmniChannel.Shares;
using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.TiktokShop.Warehouses
{
    public interface IWarehouseAppService : IApplicationService
    {
        Task<ResponseDataDto<WareHouseListDto>> GetListWareHouse();
        Task<ResponseDataDto<WareHouseListDto>> GetListWareHouseByIdShop(string idShop);
    }
}
