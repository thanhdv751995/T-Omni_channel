using OmniChannel.Brands;
using OmniChannel.Images;
using OmniChannel.ProductAttributes;
using OmniChannel.ProductDetail;
using OmniChannel.TiktokShop.Categories;
using OmniChannel.Warranty;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.ProductDetail
{
    public class ReponseProductDetailTikTokShopDto
    {
        public BrandDto Brand { get; set; }
        public List<CategoryTiktokShop> Category_list { get; set; }
        public long Create_time { get; set; }
        public string Description { get; set; }
        public List<ImageDto> Images { get; set; }
        public bool Is_cod_open { get; set; }
        public string Package_weight { get; set; }
        public List<ProductAttributeDto> Product_attributes { get; set; }
        public string Product_id { get; set; }
        public string Product_name { get; set; }
        public int Product_status { get; set; }
        public List<SkusProductDetailDto> Skus { get; set; }
        public long Update_time { get; set; }
        public WarrantyPeriodDto Warranty_period { get; set; }
        public string Warranty_policy { get; set; }
    }
}
