using OmniChannel.Shopee.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.TierVariations
{
    public class RequestInitTierVariationDto
    {
        public long item_id { get; set; }
        public List<TierVariationDto> tier_variation { get; set; }
        public List<ModelSpDto> model { get; set; }
    }
}
