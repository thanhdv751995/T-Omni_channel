using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Prices
{
    public class RequestUpdatePriceDto
    {
        public long item_id { get; set; }
        public List<PriceSPDto> price_list { get; set; }
    }
}
