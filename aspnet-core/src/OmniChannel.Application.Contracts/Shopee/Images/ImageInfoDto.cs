using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Shopee.Images
{
    public class ImageInfoDto
    {
        public string image_id { get; set; }
        public List<ImageUrlDto> image_url_list { get; set; }
    }
}
