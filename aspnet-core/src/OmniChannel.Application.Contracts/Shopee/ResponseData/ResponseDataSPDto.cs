using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.ResponseData
{
    public class ResponseDataSPDto<T>
    {
        public string error { get; set; }
        public string message { get; set; }
        public string warning { get; set; }
        public string request_id { get; set; }
        public T response { get; set; }
    }
}
