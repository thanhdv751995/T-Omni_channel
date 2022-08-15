using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class OrderDto
    {
        public string order_id { get; set; }
        public int order_status { get; set; }
        public long update_time { get; set; }

        public decimal total { get; set; }
    }
}
