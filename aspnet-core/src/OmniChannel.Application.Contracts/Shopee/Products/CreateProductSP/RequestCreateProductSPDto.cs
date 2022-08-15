using OmniChannel.Shopee.Products.CreateProductSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products
{
    public class RequestCreateProductSPDto
    {
        public string item_name { get; set; }
        public string description { get; set; }
        public float weight { get; set; }
        public float original_price { get; set; }
        public int normal_stock { get; set; }
        public List<LogisticInfoCreateProductSPDto> logistic_info { get; set; }
        public long category_id { get; set; }
        public ImageCreateProductSPDto image { get; set; }
        public BrandCreateProductSPDto brand { get; set; }
    }
}
