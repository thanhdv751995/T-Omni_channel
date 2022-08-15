using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.Shopee.Shopees
{
    public class RequestGetAccessTokenShopeeDto
    {
        public string code { get; set; }
        public long partner_id { get; set; }
        public long shop_id { get; set; }
    }
}
