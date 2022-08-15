using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.CreateProducts
{
    public class ResponseSkusDto
    {
        public string id { get; set; }
        public string seller_sku { get; set; }
        public List<ReponseSalesAttributesDto> sales_attributes { get; set; }
    }
}
