using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Attributes
{
    public class AttributeProductInfoDto
    {
        public long attribute_id { get; set; }
        public string original_attribute_name { get; set; }
        public bool is_mandatory { get; set; }
        public List<AttributeValueSPProductInfoDto> attribute_value_list { get; set; }
    }
}
