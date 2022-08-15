using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Images
{
    public class UploadFileDto
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataUploadImageDto data { get; set; }
    }
}
