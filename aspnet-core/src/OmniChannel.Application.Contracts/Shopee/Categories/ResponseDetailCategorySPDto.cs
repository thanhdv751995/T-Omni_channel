using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Categories
{
    public class ResponseDetailCategorySPDto
    {
        public long category_id { get; set; }
        public long parent_category_id { get; set; }
        public string original_category_name { get; set; }
        public string display_category_name { get; set; }
        public bool has_children { get; set; }
    }
}
