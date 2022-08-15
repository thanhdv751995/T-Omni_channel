using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Attributes
{
    public class ResponseDetailAttributeSPDto
    {
        public long attribute_id { get; set; }
        public string original_attribute_name { get; set; }
        public string display_attribute_name { get; set; }
        public bool is_mandatory { get; set; }
        public string input_validation_type { get; set; }
        public string format_type { get; set; }
        public string input_type { get; set; }
        public List<string> attribute_unit { get; set; }
        public List<AttributeValueSPDto> attribute_value_list { get; set; }
        public long max_input_value_number { get; set; }
    }
}
