using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.FulFillments.Package
{
    public class RequestShipPackageDto
    {
        public string package_id { get; set; }
        public PickUpDto pick_up { get; set; }
        public int pick_up_type { get; set; }
        public SelfShipmentDto self_shipment { get; set; }
    }
}
