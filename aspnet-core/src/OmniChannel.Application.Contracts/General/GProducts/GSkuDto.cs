using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class GSkuDto
    {
        public string Product_price { get; set; }
        public string Client_sku_id { get; set; }
        public List<GSalesCreateUpdateAttributeDto> Sales_attributes { get; set; }
    }
}
