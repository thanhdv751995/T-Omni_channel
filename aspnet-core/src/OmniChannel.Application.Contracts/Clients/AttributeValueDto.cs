using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Clients
{
    public class AttributeValueDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AttributeDto Attribute { get; set; }
    }
}
