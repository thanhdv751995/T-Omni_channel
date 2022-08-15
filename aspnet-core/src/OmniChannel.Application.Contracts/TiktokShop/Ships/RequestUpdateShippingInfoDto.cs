using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Ships
{
    public class RequestUpdateShippingInfoDto
    {
        public string order_id { get; set; }
        public string provider_id { get; set; }
        public string tracking_number { get; set; }
    }
}
