using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Brands
{
    public class ResponseGetBrandListSPDto
    {
        public List<BrandSPDto> brand_list { get; set; }
        public bool has_next_page { get; set; }
        public long next_offset { get; set; }
        public bool is_mandatory { get; set; }
        public string input_type { get; set; }
    }
}
