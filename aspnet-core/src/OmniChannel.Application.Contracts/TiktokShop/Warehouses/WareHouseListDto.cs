using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Warehouses
{
    public class WareHouseListDto
    {
        public WarehouseAddressDto Warehouse_address { get; set; }
        public int Warehouse_effect_status { get; set; }
        public string Warehouse_id { get; set; }
        public string Warehouse_name { get; set; }
        public int Warehouse_sub_type { get; set; }
        public int Warehouse_type { get; set; }
    }
}
