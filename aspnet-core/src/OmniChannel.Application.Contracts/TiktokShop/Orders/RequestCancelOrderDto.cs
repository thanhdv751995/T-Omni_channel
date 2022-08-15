using System.ComponentModel;

namespace OmniChannel.Orders
{
    public class RequestCancelOrderDto
    {
        [DefaultValue("seller_cancel_reason_wrong_price")]
        public string cancel_reason_key { get; set; }
        public string order_id { get; set; }
    }
}
