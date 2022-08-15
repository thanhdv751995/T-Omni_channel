using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public EChannel E_channel { get; set; }
        public string Category_id { get; set; }
        public string Client_category_id { get; set; }
        public string Display_name { get; set; }
        public string Parent_id { get; set; }
    }
}
