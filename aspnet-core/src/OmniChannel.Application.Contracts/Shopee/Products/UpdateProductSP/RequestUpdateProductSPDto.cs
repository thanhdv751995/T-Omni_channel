using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products.UpdateProductSP
{
    public class RequestUpdateProductSPDto
    {
        public long item_id { get; set; }
        public string item_name { get; set; }
        public long category_id { get; set; }
        public float weight { get; set; }
        public string description { get; set; }
        public UpdateImageDto image { get; set; }
    }
}
