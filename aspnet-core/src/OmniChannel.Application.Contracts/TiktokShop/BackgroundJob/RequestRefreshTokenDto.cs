using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.BackgroundJob
{
    public class RequestRefreshTokenDto
    {
        public string app_key { get; set; }
        public string app_secret { get; set; }
        public string refresh_token { get; set; }
        public string grant_type { get; set; }
    }
}
