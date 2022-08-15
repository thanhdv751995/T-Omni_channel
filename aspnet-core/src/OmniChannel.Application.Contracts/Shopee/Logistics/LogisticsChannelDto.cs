using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Logistics
{
    public class LogisticsChannelDto
    {
        public long logistics_channel_id { get; set; }
        public string logistics_channel_name { get; set; }
        public bool enabled { get; set; }
    }
}
