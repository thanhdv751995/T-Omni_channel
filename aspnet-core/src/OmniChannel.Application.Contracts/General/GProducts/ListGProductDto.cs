using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.ProductStatuss;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class ListGProductDto
    {
        public string Channel_product_id { get; set; }
        public string Client_product_id { get; set; }
        public EChannel E_channel { get; set; }
        public string E_channel_name { get; set; }
        public string Shop_id { get; set; }
        public string Shop_name { get; set; }
        public string Product_name { get; set; }
        public string Description { get; set; }
        public string Category_id { get; set; }
        public string Client_category_id { get; set; }
        public bool Is_linked { get; set; }
        public EProductStatus Product_status { get; set; }
        public string Product_status_name { get; set; }
        public DateTime Last_connection_time { get; set; }
        public List<GProductImageDto> Images { get; set; }
        public List<CreateSkusProductDto> Skus { get; set; }
    }
}
