using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class ResponseOderListDto
    {
        public bool more { get; set; }
        public string next_cursor { get; set; }
        public List<OrderDto> order_list { get; set; }
        public int total { get; set; }
    }
}
