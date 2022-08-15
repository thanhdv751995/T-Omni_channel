using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class RequestUpdatePackageShippingInfoDto
    {
        public string package_id { get; set; }
        public string provider_id { get; set; }
        public string tracking_number { get; set; }
    }
}
