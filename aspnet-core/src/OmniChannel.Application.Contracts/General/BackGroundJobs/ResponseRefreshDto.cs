using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.BackGroundJobs
{
    public class ResponseRefreshDto<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
