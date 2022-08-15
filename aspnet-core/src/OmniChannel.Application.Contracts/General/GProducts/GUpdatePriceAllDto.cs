using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class GUpdatePriceAllDto
    {
        public long Percent_increase { get; set; }
        public long Percent_decrease { get; set; }

        public long Same_price { get; set; }
    }
}
