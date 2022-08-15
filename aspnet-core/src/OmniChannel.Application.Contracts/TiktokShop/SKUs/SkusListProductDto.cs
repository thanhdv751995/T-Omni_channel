using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Products
{
    public class SkusListProductDto
    {
        public string id { get; set; }
        public string seller_sku { get; set; }
        public PriceProductDto price { get; set; }
        public List<StockInfosDtoCreateProduct> stock_infos { get; set; }
    }
}
