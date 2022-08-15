using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.ProductAttributes
{
    public class ProductAttributeDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ValueAttribute> values { get; set; }
    }
}
