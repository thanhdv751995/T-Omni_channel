using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Files
{
    public class UrlFileDto : IFileDto
    {
        public string Content { get; set; }
        public string FileName { get; set; }
        public string Extention { get; set; }
        public long Size { get; set; }
        public string Type { get; private set; } = "Url";
    }
}
