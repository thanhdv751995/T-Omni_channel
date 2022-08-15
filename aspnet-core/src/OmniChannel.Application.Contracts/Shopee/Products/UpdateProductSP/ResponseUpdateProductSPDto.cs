using OmniChannel.Shopee.Products.CreateProductSP;
using OmniChannel.Shopee.TierVariations;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products.UpdateProductSP
{
    public class ResponseUpdateProductSPDto
    {
        public long item_id { get; set; }
        public long category_id { get; set; }
        public string item_name { get; set; }
        public string description { get; set; }
        public PriceInfoDto price_info { get; set; }
        public StockInfoDto stock_info { get; set; }
        public ImageProductInfoBaseDto images { get; set; }
        public float weight { get; set; }
        public DimensionProductInfoBaseDto dimension { get; set; }
        public List<LogisticInfoDto> logistic_info { get; set; }
        public PreOrderDto pre_order { get; set; }
        public string condition { get; set; }
        public string item_status { get; set; }
        public BrandDto brand { get; set; }
        public string description_type { get; set; }
        public List<SellerStockDto> seller_stock { get; set; }
        public ResponseCreateTierVariationDto model { get; set; }

    }
}
