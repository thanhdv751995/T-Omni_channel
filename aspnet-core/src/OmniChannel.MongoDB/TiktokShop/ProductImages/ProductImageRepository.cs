using OmniChannel.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.TiktokShop.ProductImages
{
    public class ProductImageRepository : MongoDbRepository<OmniChannelMongoDbContext, ProductImage, Guid>, IProductImageRepository
    {
        public ProductImageRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
