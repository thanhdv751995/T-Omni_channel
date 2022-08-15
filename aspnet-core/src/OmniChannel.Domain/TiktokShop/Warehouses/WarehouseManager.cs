using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.TiktokShop.Warehouses
{
    public class WarehouseManager : DomainService
    {
        public WarehouseManager()
        {
        }
        public Warehouse CreateAsync(
                WarehouseAddressDto warehouse_address,
                 int warehouse_effect_status,
                 string warehouse_id,
                 string warehouse_name,
                 int warehouse_sub_type,
                 int warehouse_type
           )
        {
            return new Warehouse(
               GuidGenerator.Create(),
               warehouse_address,
               warehouse_effect_status,
               warehouse_id,
               warehouse_name,
               warehouse_sub_type,
               warehouse_type
            );
        }
    }
}
