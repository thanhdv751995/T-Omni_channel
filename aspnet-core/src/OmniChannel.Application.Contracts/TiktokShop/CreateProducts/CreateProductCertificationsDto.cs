using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Products;

namespace OmniChannel.CreateProducts
{
    public class CreateProductCertificationsDto
    {
        public string id { get; set; }
        public List<CreateImagesIdDto> images { get; set; }
        public List<CreateFilesProductDto> files { get; set; }
    }
}
