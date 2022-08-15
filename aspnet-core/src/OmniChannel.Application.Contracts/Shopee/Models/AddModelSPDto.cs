using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Models
{
    public class AddModelSPDto
    {
        public long item_id { get; set; }
        public List<ModelSpDto> model_list { get; set; }
    }
}
