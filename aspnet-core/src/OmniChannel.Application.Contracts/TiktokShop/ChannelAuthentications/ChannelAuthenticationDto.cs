using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.ChannelAuthentications
{
    public class ChannelAuthenticationDto
    {
        public string Channel_token { get; set; }
        public string Access_token { get; set; }
        public string Access_token_expire_in { get; set; }
        public string Refresh_token { get; set; }
        public string Refresh_token_expire_in { get; set; }
        public string Open_id { get; set; }
        public string Seller_name { get; set; }
        public string Shop_id { get; set; }
        public string App { get; set; }
        public string Client_id { get; set; }
        public DateTime CreationTime { get; set; }
        public List<WarehouseDto> Warehouse_list { get; set; }
    }
}
