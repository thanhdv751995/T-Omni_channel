using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.LinkedProducts
{
    public class AddLinkedProductDto
    {
        public string Channel_product_id { get; set; }
        public string Client_product_id { get; set; }
        public string Client_data { get; set; }
    }
}
