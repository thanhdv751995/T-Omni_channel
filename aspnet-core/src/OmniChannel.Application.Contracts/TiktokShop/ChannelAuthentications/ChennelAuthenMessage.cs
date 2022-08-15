using OmniChannel.General.Shops;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.ChannelAuthentications
{
    public class ChennelAuthenMessage
    {
        public string Message { get; set; }
        public string Channel_token { get; set; }
        public GShopDto Shop_data { get; set; }
    }
}
