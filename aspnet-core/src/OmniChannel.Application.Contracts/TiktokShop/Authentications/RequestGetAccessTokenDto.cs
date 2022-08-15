using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Authentications
{
    public class RequestGetAccessTokenDto
    {
        public string App_key { get; set; }
        public string App_secret { get; set; }
        public string Auth_code { get; set; }
        public string Grant_type { get; set; } = "authorized_code";
    }
}
