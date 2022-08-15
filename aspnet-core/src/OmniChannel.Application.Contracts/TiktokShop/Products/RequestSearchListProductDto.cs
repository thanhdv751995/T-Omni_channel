using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Products
{
    public class RequestSearchListProductDto
    {
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = int.MaxValue;
        //public string create_time_from { get; set; }
        //public string create_time_to { get; set; }
        //public string search_status { get; set; }
        //public string seller_sku_list { get; set; }

        //public string update_time_from { get; set; }
        //public string update_time_to { get; set; }
    }
}
