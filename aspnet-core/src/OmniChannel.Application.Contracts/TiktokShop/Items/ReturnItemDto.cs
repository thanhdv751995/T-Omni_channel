using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Items
{
    public class ReturnItemDto
    {
        public string product_images { get; set; }
        public string return_product_id { get; set; }
        public string return_product_name { get; set; }
        public string return_quantity { get; set; }
        public string seller_sku { get; set; }
        public string sku_id { get; set; }
        public string sku_name { get; set; }
    }
}
