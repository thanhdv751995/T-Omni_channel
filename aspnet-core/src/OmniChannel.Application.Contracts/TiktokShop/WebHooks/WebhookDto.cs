using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.WebHooks
{
    public class WebhookDto
    {
        public int Type { get; set; }
        public string Shop_id { get; set; }
        public int TimeStamp { get; set; }
        public DataWebhookDto Data { get; set; }
    }
}
