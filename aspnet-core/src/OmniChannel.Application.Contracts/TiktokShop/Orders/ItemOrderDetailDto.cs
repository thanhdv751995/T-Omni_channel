using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class ItemOrderDetailDto
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public string seller_sku { get; set; }
        public string sku_cancel_reason { get; set; }
        public string sku_cancel_user { get; set; }
        public int sku_display_status { get; set; }
        public int sku_ext_status { get; set; }
        public string sku_id { get; set; }
        public string sku_image { get; set; }
        public string sku_name { get; set; }
        public int sku_original_price { get; set; }
        public int sku_platform_discount { get; set; }
        public int sku_sale_price { get; set; }
        public int sku_seller_discount { get; set; }
        public int sku_type { get; set; }
    }
}
