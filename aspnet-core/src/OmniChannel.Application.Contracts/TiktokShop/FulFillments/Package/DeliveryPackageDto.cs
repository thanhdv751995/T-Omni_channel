using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class DeliveryPackageDto
    {
        public int delivery_type { get; set; }
        public int file_type { get; set; }
        public string file_url { get; set; }
        public long package_id { get; set; }
        public string reason { get; set; }
    }
}
