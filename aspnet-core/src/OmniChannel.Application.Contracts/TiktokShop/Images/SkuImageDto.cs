using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Images
{
    public class SkuImageDto
    {
        public int height { get; set; }
        public string id { get; set; }
        public List<string> thumb_url_list { get; set; }
        public List<string> url_list { get; set; }
        public int width { get; set; }
    }
}
