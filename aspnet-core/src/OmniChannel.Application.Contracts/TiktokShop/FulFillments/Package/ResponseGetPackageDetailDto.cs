using OmniChannel.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class ResponseGetPackageDetailDto
    {
        public long create_time { get; set; }
        public string delivery_option { get; set; }
        public int note_tag { get; set; }
        public List<OrderInfoDto> order_info_list { get; set; }
        public string order_line_id_list { get; set; }
        public int package_freeze_status { get; set; }
        public string package_id { get; set; }
        public int package_status { get; set; }
        public long pick_up_end_time { get; set; }
        public long pick_up_start_time { get; set; }
        public int pick_up_type { get; set; }
        public int print_tag { get; set; }
        public int sc_tag { get; set; }
        public string shipping_provider { get; set; }
        public string shipping_provider_id { get; set; }
        public string sku_tag { get; set; }
        public string tracking_number { get; set; }
        public long update_time { get; set; }
    }
}
