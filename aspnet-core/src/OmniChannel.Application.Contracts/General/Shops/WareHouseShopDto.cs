using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.Shops
{
    public class WareHouseShopDto
    {
        public WarehouseAddressDto Warehouse_address { get; set; }
        public int Warehouse_effect_status { get; set; }
        public string Warehouse_effect_status_name { get; set; }
        public string Warehouse_id { get; set; }
        public string Warehouse_name { get; set; }
        public int Warehouse_sub_type { get; set; }
        public string Warehouse_sub_type_name { get; set; }
        public int Warehouse_type { get; set; }
        public string Warehouse_type_name { get; set; }
    }
}
