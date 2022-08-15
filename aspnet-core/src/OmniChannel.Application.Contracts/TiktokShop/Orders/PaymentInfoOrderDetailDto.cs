using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class PaymentInfoOrderDetailDto
    {
        public string currency { get; set; }
        public int original_shipping_fee { get; set; }
        public int original_total_product_price { get; set; }
        public int platform_discount { get; set; }
        public int seller_discount { get; set; }
        public int shipping_fee { get; set; }
        public int shipping_fee_platform_discount { get; set; }
        public int shipping_fee_seller_discount { get; set; }
        public int sub_total { get; set; }
        public int taxes { get; set; }
        public int total_amount { get; set; }
    }
}
