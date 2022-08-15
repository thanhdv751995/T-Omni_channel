using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Items;
using OmniChannel.Ships;

namespace OmniChannel.Deliveries
{
    public class ResponseDeliveryOptionDto
    {
        public string delivery_option_id { get; set; }
        public string delivery_option_name { get; set; }
        public ItemDimensionLimitDto item_dimension_limit { get; set; }
        public ItemWeightLimitDto item_weight_limit { get; set; }
        public List<ShippingProviderDto> shipping_provider_list { get; set; }
    }
}
