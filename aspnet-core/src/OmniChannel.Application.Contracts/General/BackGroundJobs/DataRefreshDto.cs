using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.BackGroundJobs
{
    public class DataRefreshDto
    {
        public string Access_token { get; set; }
        public string Access_token_expire_in { get; set; }
        public string Refresh_token { get; set; }
        public string Refresh_token_expire_in { get; set; }
        public string Open_id { get; set; }
        public string Seller_name { get; set; }
        public string Request_id { get; set; }
    }
}
