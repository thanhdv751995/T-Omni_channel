using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Categories
{
    public class ResponseDetailCategoryDto
    {
        public string id { get; set; }
        public bool is_leaf { get; set; }
        public string local_display_name { get; set; }
        public string parent_id { get; set; }
    }
}
