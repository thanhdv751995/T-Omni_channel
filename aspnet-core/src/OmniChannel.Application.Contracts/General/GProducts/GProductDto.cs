using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class GProductDto
    {
        public string Channel_product_id { get; set; }
        public EChannel E_channel { get; set; }
        public string E_channel_name { get; set; }
        public string Product_name { get; set; }
        public string Description { get; set; }
        public string Category_id { get; set; }
        public string Original_price { get; set; }
        public int Available_stock { get; set; }
        public List<CreateImagesIdDto> Images { get; set; }
        public List<GSalesAttributeDto> Sales_attributes { get; set; }
    }
}
