using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class RequestOrderIdList
    {
        public RequestOrderIdList()
        {
            order_id_list = new List<string>();
        }

        public List<string> order_id_list { get; set; }
    }
}
