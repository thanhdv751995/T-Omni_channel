using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Products
{
    public class ResponseProductListDto
    {
        public List<ProductDetailListDto> item { get; set; }
        public long total_count { get; set; }
        public bool has_next_page { get; set; }
    }
}
