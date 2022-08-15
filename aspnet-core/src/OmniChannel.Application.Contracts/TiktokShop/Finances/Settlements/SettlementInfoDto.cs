using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances
{
    public class SettlementInfoDto
    {
        public string affiliate_commission { get; set; }
        public string charge_back { get; set; }
        public string currency { get; set; }
        public string customer_service_compensation { get; set; }
        public string flat_fee { get; set; }
        public string other_adjustment { get; set; }
        public string payment_fee { get; set; }
        public string platform_commission { get; set; }
        public string platform_promotion { get; set; }
        public string promotion_adjustment { get; set; }
        public string refund { get; set; }
        public string sales_fee { get; set; }
        public string settlement_amount { get; set; }
        public long settlement_time { get; set; }
        public string shipping_fee { get; set; }
        public string shipping_fee_adjustment { get; set; }
        public string shipping_fee_subsidy { get; set; }
        public string user_pay { get; set; }
        public string vat { get; set; }
    }
}
