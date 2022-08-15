using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.Shops
{
    public class GShopDto
    {
        public string Channel_token { get; set; }
        public string Shop_id { get; set; }
        public string Shop_name { get; set; }
        public DateTime Last_connected_time  { get; set; }
        public bool Is_active { get; set; }
        public EChannel E_Channel { get; set; }
    }
}
