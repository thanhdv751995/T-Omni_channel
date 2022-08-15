using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General
{
    public class GResponse<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }
    }
}
