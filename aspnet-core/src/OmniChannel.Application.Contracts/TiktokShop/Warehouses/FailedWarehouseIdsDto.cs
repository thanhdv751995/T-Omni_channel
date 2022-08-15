using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Warehouses
{
    public class FailedWarehouseIdsDto
    {
        public string id { set; get; }
        public List<string> failed_warehouse_ids { get; set; }
    }
}
