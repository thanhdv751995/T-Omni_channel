using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Reverses
{
    public class RequestReverseOrderListDto
    {
        //public long update_time_from { get; set; }
        //public long update_time_to { get; set; }
        //public int reverse_type { get; set; }
        //public int sort_by { get; set; }
        //public int sort_type { get; set; }
        public int offset { get; set; }
        public int size { get; set; }
        //public int reverse_order_status { get; set; }
        public int order_id { get; set; }
        public int reverse_order_id { get; set; }
    }
}
