using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Attributes
{
    public class DetailAttributeDto
    {
        public int Attribute_type { get; set; }
        public string Id { get; set; }
        public InputTypeDto Input_type { get; set; }
        public string Name { get; set; }
        public List<ValueAttributeDto> Values { get; set; }
    }
}
