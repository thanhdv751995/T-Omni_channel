using OmniChannel.Channels;
using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.ChannelAuthentications
{
    public class CreateUpdateChannelAuthenticationDto
    {
        public string Access_token { get; set; }
        public string Access_token_expire_in { get; set; }
        public string Refresh_token { get; set; }
        public string Refresh_token_expire_in { get; set; }
        public string Open_id { get; set; }
        public string Seller_name { get; set; }
        public string Shop_id { get; set; }
        public string App { get; set; }
        public string Client_id { get; set; }
        public List<WarehouseDto> Warehouses { get; set; }
        public EChannel E_Channel { get; set; }
    }
}
