using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products
{
    public class LogisticInfoDto
    {
        public long logistic_id { get; set; }
        public string logistic_name { get; set; }
        public bool enabled { get; set; }
        public bool is_free { get; set; }
        public long estimated_shipping_fee { get; set; }
    }
}
