using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Split
{
    public class RequestOrderConfirmSplitDto
    {
        public long order_id { get; set; }
        public List<SplitGroupDto> split_group { get; set; }
    }
}
