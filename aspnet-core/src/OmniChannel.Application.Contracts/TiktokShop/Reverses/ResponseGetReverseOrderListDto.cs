using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Reverses
{
    public class ResponseGetReverseOrderListDto
    {
        public bool more { get; set; }
        public List<ReverseDto> reverse_list { get; set; }
    }
}
