using OmniChannel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.LinkedProducts
{
    public class ProductLinkedListDto
    {
        public long Create_time { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public long Update_time { get; set; }
        public string Shop_id { get; set; }
        public string Shop_name { get; set; }
        public bool Is_linked { get; set; }
        public List<string> Sale_regions { get; set; }
        public List<SkusListProductDto> Skus { get; set; }
    }
}
