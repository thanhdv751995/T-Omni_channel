using OmniChannel.Shopee.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products
{
    public class ProductBaseInfoDto
    {
        public long item_id { get; set; }
        public long category_id { get; set; }
        public string item_name { get; set; }
        public string description { get; set; }
        public string item_sku { get; set; }
        public long create_time { get; set; }
        public long update_time { get; set; }
        public List<AttributeProductInfoDto> attribute_list { get; set; }
        public ImageProductInfoBaseDto image { get; set; }
        public string weight { get; set; }
        public DimensionProductInfoBaseDto dimension { get; set; }
        public List<LogisticInfoDto> logistic_info { get; set; }
        public PreOrderDto pre_order { get; set; }
        public string condition { get; set; }
        public string size_chart { get; set; }
        public string item_status { get; set; }
        public bool has_model { get; set; }
        public BrandDto brand { get; set; }
        public long item_dangerous { get; set; }
        public string description_type { get; set; }
    }
}
