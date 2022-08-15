using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Authentications
{
    public class ChannelAuthenticationShopeeDto
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public long refresh_token_expire_in { get; set; }
        public string seller_name { get; set; }
        public long shop_id { get; set; }
        public long open_id { get; set; }
        public EChannel e_channel { get; set; }
        public long access_token_expire_in { get; set; } = 0;
    }
}
