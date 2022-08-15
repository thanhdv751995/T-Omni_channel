using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Files
{
    public class Base64FileDto : IFileDto
    {
        public string Content { get; set; }
        public string Extention { get; set; }
        public long Size { get; set; }
        public string Type { get; private set; } = "Base64";
    }
}
