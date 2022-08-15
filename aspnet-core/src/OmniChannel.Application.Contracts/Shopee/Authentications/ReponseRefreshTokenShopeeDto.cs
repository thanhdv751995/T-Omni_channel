using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Authentications
{
    public class ReponseRefreshTokenShopeeDto
    {
        public string request_id { get; set; }
        public string error { get; set; }
        public string refresh_token { get; set; }
        public string access_token { get; set; }
        public int expire_in { get; set; }
        public int partner_id { get; set; }
        public int shop_id { get; set; }
        public int merchant_id { get; set; }


    }
}
