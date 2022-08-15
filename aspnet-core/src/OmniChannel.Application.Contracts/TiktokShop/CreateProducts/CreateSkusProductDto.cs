using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Products;

namespace OmniChannel.CreateProducts
{
    public class CreateSkusProductDto
    {
        public string Id { get; set; }
        public string Client_sku_id { get; set; }
        public List<SalesAttributesDto> Sales_attributes { get; set; } 
        public List<CreateStockInfosProductDto> Stock_infos { get; set; }
        public string Seller_sku { get; set; }
        public string Original_price { get; set; }
    }
}
