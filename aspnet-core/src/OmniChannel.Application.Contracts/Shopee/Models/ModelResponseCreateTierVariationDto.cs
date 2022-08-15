using OmniChannel.Shopee.Prices;
using OmniChannel.Shopee.Stocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Models
{
    public class ModelResponseCreateTierVariationDto
    {
        public List<int> tier_index { get; set; }
        public long model_id { get; set; }
        public string client_sku_id { get; set; }
        public List<StockInfoSPDto> stock_info { get; set; }
        public List<PriceInfoSPDto> price_info { get; set; }
        public List<SellerStockSPDto> seller_stock { get; set; }
    }
}
