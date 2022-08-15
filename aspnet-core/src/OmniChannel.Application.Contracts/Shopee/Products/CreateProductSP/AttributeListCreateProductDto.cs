using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products.CreateProductSP
{
    public class AttributeListCreateProductDto
    {
        public long attribute_id { get; set; }
        public List<AttributeValueCreateProductDto> attribute_value_list { get; set; }
    }
}
