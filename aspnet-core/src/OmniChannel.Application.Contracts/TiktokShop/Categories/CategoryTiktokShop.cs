using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.TiktokShop.Categories
{
    public class CategoryTiktokShop
    {
        public string id { get; set; }
        public bool is_leaf { get; set; }
        public string display_name { get; set; }
        public string parent_id { get; set; }
    }
}
