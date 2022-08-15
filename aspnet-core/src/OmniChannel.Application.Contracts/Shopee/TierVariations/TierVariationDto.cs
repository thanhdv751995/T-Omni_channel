using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.TierVariations
{
    public class TierVariationDto
    {
        public string name { get; set; }
        public List<OptionDto> option_list { get; set; }
    }
}
