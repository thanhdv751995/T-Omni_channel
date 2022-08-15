using OmniChannel.SKUs;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class OrderInfoDto
    {
        public string order_id { get; set; }
        public List<SkuDto> sku_list { get; set; }
    }
}
