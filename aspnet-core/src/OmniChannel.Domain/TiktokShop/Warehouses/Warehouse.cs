using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.TiktokShop.Warehouses
{
    public class Warehouse : AuditedAggregateRoot<Guid>
    {
        public WarehouseAddressDto Warehouse_address { get; set; }
        public int Warehouse_effect_status { get; set; }
        public string Warehouse_id { get; set; }
        public string Warehouse_name { get; set; }
        public int Warehouse_sub_type { get; set; }
        public int Warehouse_type { get; set; }

        private Warehouse()
        {

        }
        internal Warehouse(
                 Guid id,
                 WarehouseAddressDto warehouse_address,
                 int warehouse_effect_status,
                 string warehouse_id,
                 string warehouse_name,
                 int warehouse_sub_type,
                 int warehouse_type
                )
                : base(id)
        {
            Warehouse_address = warehouse_address;
            Warehouse_effect_status = warehouse_effect_status;
                Warehouse_id = warehouse_id;
                Warehouse_name = warehouse_name;
                Warehouse_sub_type = warehouse_sub_type;
                Warehouse_type = warehouse_type;              
        }
    }
}
