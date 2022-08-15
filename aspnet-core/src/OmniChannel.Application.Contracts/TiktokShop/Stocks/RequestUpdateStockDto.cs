using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.SKUs;

namespace OmniChannel.Stocks
{
    public class RequestUpdateStockDto
    {
        public string product_id { get; set; }
        public List<RequestUpdateSkusStockDto> skus { get; set; }
    }
}
