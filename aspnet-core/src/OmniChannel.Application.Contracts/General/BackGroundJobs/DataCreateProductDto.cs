using OmniChannel.CreateProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.BackGroundJobs
{
    public class DataCreateProductDto
    {
        public string Product_name { get; set; }
        public string Description { get; set; }
        public string Category_id { get; set; }
        public string Brand_id { get; set; }
        public List<CreateImagesIdDto> Images { get; set; }
        public int Warranty_period { get; set; }
        public string Warranty_policy { get; set; }
        public int Package_length { get; set; }
        public int Package_width { get; set; }
        public int Package_height { get; set; }
        public string Package_weight { get; set; }
        public CreateSizeChartImgIdDto Size_chart { get; set; }
        public List<CreateProductCertificationsDto> Product_certifications { get; set; }
        public bool Is_cod_open { get; set; }
        public List<CreateSkusProductDto> Skus { get; set; }
        public List<string> Delivery_service_ids { get; set; }
        public List<CreateProductAttributesDto> Product_attributes { get; set; }
    }
}
