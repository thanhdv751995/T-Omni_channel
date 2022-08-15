using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Products
{
    public class ResponseListProductDto
    {
        public int Total { get; set; }
        public List<DetailProductListDto> Products { get; set; }
    }
}
