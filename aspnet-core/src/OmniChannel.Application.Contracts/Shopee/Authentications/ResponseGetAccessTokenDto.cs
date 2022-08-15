using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Authentications
{
    public class ResponseGetAccessTokenDto
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public long expire_in { get; set; }
        public long refresh_token_expire_in { get; set; } = 0;
    }
}
