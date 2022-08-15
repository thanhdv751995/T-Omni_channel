using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Products;
using OmniChannel.SalesAttributes;

namespace OmniChannel.ProductDetail
{
    public class SkusProductDetailDto
    {
        public string id { get; set; }
        public PriceProductDto price { get; set; }
        public List<SalesAttributeDto> sales_attributes { get; set; }
        public string seller_sku { get; set; }
        public List<StockInfosDtoCreateProduct> stock_infos { get; set; }
    }
}
