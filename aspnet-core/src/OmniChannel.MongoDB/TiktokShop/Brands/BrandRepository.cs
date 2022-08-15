using OmniChannel.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.Brands
{
    public class BrandRepository : MongoDbRepository<OmniChannelMongoDbContext, Brand, string>, IBrandRepository
    {
        public BrandRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
