using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.CreateProducts
{
    public class CreateProductAttributesDto
    {
        public string attribute_id { get; set; }
        public CreateAttributeValuesDto attribute_values { get; set; }
    }
}
