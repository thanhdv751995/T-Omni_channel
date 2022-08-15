using OmniChannel.Shopee.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.TierVariations
{
    public class ResponseCreateTierVariationDto
    {
        public long item_id { get; set; }
        public List<TierVariationDto> tier_variation { get; set; }
        public List<ModelResponseCreateTierVariationDto> model { get; set; }
    }
}
