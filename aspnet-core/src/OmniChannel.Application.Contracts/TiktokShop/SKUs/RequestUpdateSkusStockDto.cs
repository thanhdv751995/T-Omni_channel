using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Products;

namespace OmniChannel.SKUs
{
    public class RequestUpdateSkusStockDto
    {
        public string id { get; set; }
        public List<StockInfosDtoCreateProduct> stock_infos { get; set; }
    }
}
