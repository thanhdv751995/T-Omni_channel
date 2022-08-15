using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Products
{
    public class DetailProductListDto
    {
        public long Create_time { get; set; }
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public long Update_time { get; set; }
        public List<string> Sale_regions { get; set; }
        public List<SkusListProductDto> Skus { get; set; }
    }
}
