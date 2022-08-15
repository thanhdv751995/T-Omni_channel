using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.CreateProducts
{
    public class CreateProductMapDto
    {
        public string product_name { get; set; }
        public string description { get; set; }
        public string category_id { get; set; }
        public string brand_id { get; set; }
        public List<CreateImagesIdDto> images { get; set; }
        public int warranty_period { get; set; }
        public string warranty_policy { get; set; }
        public int package_length { get; set; }
        public int package_width { get; set; }
        public int package_height { get; set; }
        public string package_weight { get; set; }
        public CreateSizeChartImgIdDto size_chart { get; set; }
        public List<CreateProductCertificationsDto> product_certifications { get; set; }
        public bool is_cod_open { get; set; }
        public List<CreateSkusProductDto> skus { get; set; }
        public List<string> delivery_service_ids { get; set; }
        public List<CreateProductAttributesDto> product_attributes { get; set; }
    }
}
