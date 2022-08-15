using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Images
{
    public class ImageDto
    {
        public int Height { get; set; }
        public string Id { get; set; }
        public List<string> Thumb_url_list { get; set; }
        public List<string> Url_list { get; set; }
        public int Width { get; set; }
    }
}
