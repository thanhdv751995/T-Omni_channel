using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Split
{
    public class SplitGroupDto
    {
        public List<int> order_line_id_list { get; set; }
        public int pre_split_pkg_id { get; set; }
    }
}
