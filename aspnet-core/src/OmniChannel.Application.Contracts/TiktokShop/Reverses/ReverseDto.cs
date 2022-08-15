using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Items;

namespace OmniChannel.Reverses
{
    public class ReverseDto
    {
        public string currency { get; set; }
        public string order_id { get; set; }
        public string refund_total { get; set; }
        public List<ReturnItemDto> return_item_list { get; set; }
        public string return_reason { get; set; }
        public string return_tracking_id { get; set; }
        public string reverse_order_id { get; set; }
        public List<ReverseRecordDto> reverse_record_list { get; set; }
        public long reverse_request_time { get; set; }
        public int reverse_status_value { get; set; }
        public int reverse_type { get; set; }
        public int reverse_update_time { get; set; }
    }
}
