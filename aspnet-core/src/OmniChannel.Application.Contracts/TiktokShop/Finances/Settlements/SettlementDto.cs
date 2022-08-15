using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances
{
    public class SettlementDto
    {
        public string adjustment_id { get; set; }
        public string order_id { get; set; }
        public string product_name { get; set; }
        public string related_order_id { get; set; }
        public SettlementInfoDto settlement_info { get; set; }
        public string sku_id { get; set; }
        public string sku_name { get; set; }
        public string unique_key { get; set; }
    }
}
