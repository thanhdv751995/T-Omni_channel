using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShops
{
    public class TiktokShopDto
    {
        public string TiktokShopAuthorizeDomain { get; set; }
        public string TiktokShopShopDomain { get; set; }
        public string app_key { get; set; }
        public string app_secret { get; set; }
        public string crypto_secret_key { get; set; }
    }
}
