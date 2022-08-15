using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.TiktokShop.ProductImages
{
    public class ProductImageManager : DomainService
    {
        public ProductImageManager()
        {
        }
        public ProductImage CreateAsync(
               string img_id,
               string img_url,
               int img_scene
           )
        {
            return new ProductImage(
               GuidGenerator.Create(),
               img_id,
               img_url,
               img_scene

            );
        }
    }
}
