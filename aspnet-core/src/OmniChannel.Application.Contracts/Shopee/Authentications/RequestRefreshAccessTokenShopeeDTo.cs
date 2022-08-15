using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Authentications
{
    public class RequestRefreshAccessTokenShopeeDTo
    {
        public string Refresh_token { get; set; }
        public int Partner_id { get; set; }
        public int Shop_id { get; set; }
    }
}
