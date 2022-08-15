using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.CreateProducts
{
    public class ResponseDataDto
    {
        public string product_id { get; set; }
        public List<ResponseSkusDto> skus { get; set; }
    }
}
