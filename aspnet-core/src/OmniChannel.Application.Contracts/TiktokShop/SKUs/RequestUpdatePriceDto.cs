using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.SKUs
{
    public class RequestUpdatePriceDto
    {
        public string product_id { get; set; }
        public List<OriginalPriceDto> skus { get; set; }
    }
}
