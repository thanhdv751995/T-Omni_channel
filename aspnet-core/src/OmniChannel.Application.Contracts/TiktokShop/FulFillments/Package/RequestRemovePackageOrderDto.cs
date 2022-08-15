using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class RequestRemovePackageOrderDto
    {
        public List<string> order_id_list { get; set; }
        public string package_id { get; set; }
    }
}
