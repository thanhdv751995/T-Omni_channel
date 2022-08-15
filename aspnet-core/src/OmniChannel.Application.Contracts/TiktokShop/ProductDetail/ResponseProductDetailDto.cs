using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Brands;
using OmniChannel.Categories;
using OmniChannel.Images;
using OmniChannel.ProductAttributes;
using OmniChannel.Warranty;

namespace OmniChannel.ProductDetail
{
    public class ResponseProductDetailDto
    {
        public BrandDto brand { get; set; }
        public List<CategoryDto> category_list { get; set; }
        public long create_time { get; set; }
        public string description { get; set; }
        public List<ImageDto> images { get; set; }
        public bool is_cod_open { get; set; }
        public string package_weight { get; set; }
        public List<ProductAttributeDto> product_attributes { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public int product_status { get; set; }
        public List<SkusProductDetailDto> skus { get; set; }
        public long update_time { get; set; }
        public WarrantyPeriodDto warranty_period { get; set; }
        public string warranty_policy { get; set; }
    }
}
