using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shares
{
    public class ResponseDataDto<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Request_id { get; set; }
        public T Data { get; set; }
    }
}
