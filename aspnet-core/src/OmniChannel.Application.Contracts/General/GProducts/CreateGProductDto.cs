using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class CreateGProductDto
    {
        public EChannel E_channel { get; set; }
        public string Product_name { get; set; }
        public int Product_status { get; set; }
        public string Description { get; set; }
        public List<string> Image_ids { get; set; }
        public string Package_weight { get; set; }
        public int? Available_stock { get; set; }
        public List<GSkuDto> Skus { get; set; }
        public string Client_product_id { get; set; }
        public string Client_category_id { get; set; }
        public string Client_data { get; set; }
    }
}
