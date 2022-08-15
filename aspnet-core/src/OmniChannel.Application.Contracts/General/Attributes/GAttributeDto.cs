using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.Attributes
{
    public class GAttributeDto
    {
        public Guid Id { get; set; }
        public EChannel e_Channel { get; set; }
        public string Name { get; set; }
        public string Attribute_id { get; set; }
        public string Category_id { get; set; }
        public string Client_attribute_id { get; set; }
    }
}
