using OmniChannel.Attributes;
using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.Attributes
{
    public class CreateAttributeDto
    {
        public EChannel E_channel { get; set; }
        public string Attribute_id { get; set; }
        public string Category_id { get; set; }
        public string Name { get; set; }
        public string Client_attribute_id { get; set; }
    }
}
