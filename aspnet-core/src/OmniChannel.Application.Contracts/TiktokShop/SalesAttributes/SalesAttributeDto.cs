using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Images;

namespace OmniChannel.SalesAttributes
{
    public class SalesAttributeDto
    {
        public string id { get;set; }
        public string name { get;set; }
        public SkuImageDto sku_img { get;set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
    }
}
