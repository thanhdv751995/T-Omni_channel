using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.TiktokShop.ProductImages
{
    public class ProductImage : AuditedAggregateRoot<Guid>
    {
        public string Img_id { get; set; }
        public string Img_url { get; set; }
        public int Img_scene { get; set; }

        private ProductImage()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal ProductImage(
               Guid id,
               string img_id,
               string img_url,
               int img_scene
           )
           : base(id)
        {
            Img_id = img_id;
            Img_url = img_url;
            Img_scene = img_scene;
        }
    }
}
