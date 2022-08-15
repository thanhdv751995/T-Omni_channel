using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Orders
{
    public class RecipientAddressOrderDetailDto
    {
        public string address_detail { get; set; }
        public List<string> address_line_list { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string full_address { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public string state { get; set; }
        public string town { get; set; }
        public string zipcode { get; set; }
    }
}
