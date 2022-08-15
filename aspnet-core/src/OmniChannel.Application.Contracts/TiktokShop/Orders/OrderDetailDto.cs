using System.Collections.Generic;

namespace OmniChannel.Orders
{
    public class OrderDetailDto
    {
        public string buyer_message { get; set; }
        public string buyer_uid { get; set; }
        public long cancel_order_sla { get; set; }
        public string cancel_reason { get; set; }
        public string cancel_user { get; set; }
        public long create_time { get; set; }
        public string delivery_option { get; set; }
        public int ext_status { get; set; }
        public int fulfillment_type { get; set; }
        public long paid_time { get; set; }
        public List<ItemOrderDetailDto> item_list { get; set; }
        public string order_id { get; set; }
        public List<OrderLineOrderDetailDto> order_line_list { get; set; }
        public int order_status { get; set; }
        public List<PakageOrderDetailDto> package_list { get; set; }
        public PaymentInfoOrderDetailDto payment_info { get; set; }
        public string payment_method { get; set; }
        public int receiver_address_updated { get; set; }
        public RecipientAddressOrderDetailDto recipient_address { get; set; }
        public long rts_sla { get; set; }
        public string shipping_provider { get; set; }
        public string shipping_provider_id { get; set; }
        public string tracking_number { get; set; }
        public long tts_sla { get; set; }
        public long update_time { get; set; }
    }
}
