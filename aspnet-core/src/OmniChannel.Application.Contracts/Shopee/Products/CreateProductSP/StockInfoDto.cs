using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products.CreateProductSP
{
    public class StockInfoDto
    {
        public int stock_type { get; set; }
        public long normal_stock { get; set; }
        public long current_stock { get; set; }
    }
}
