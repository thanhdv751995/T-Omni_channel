using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.Shops
{
    public class ShopDetailDto
    {
        public string Shop_name { get; set; }
        public string Shop_id { get; set; } 
        public string Company_name { get; set; }
        public string Tax_code { get; set; }
        public List<WareHouseShopDto> Warehouse_list { get; set; }      
    }
}
