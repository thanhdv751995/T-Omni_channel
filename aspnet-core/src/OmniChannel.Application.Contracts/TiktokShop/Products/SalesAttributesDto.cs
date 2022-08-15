using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.CreateProducts;

namespace OmniChannel.Products
{
    public class SalesAttributesDto
    {
        public Guid Id { get; set; }
        public string Attribute_id { get; set; }
        public string Custom_value { get; set; }
        public List<CreateImagesIdDto> Sku_img { get; set; }
    }
}
