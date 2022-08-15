using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.CreateProducts
{
    public class ResponseCreateProductDto
    {
        public string code { get; set; }
        public string message { get; set; }
        public ResponseDataDto data { get; set; }
    }
}
