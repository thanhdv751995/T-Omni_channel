using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.WebHooks
{
    public class DataWebhookDto
    {
        public string Product_id { get; set; }
        public string Status { get; set; }
        public string Suspended_reason { get; set; }
        public int Update_time { get; set; }
    }
}
