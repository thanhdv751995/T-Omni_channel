using MongoDB.Bson;
using MongoDB.Driver;
using OmniChannel.Channels;
using OmniChannel.MongoDB;
using OmniChannel.TiktokShop.Hosted;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.Categories
{
    public class CategoryRepository : MongoDbRepository<OmniChannelMongoDbContext, Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<long> GetCountAsync(EChannel e_channel)
        {
            return await HostedAppService._categoryCollection.CountDocumentsAsync(x => x.E_Channel == e_channel);
        }

        public async Task<List<Category>> GetListAsync(EChannel e_Channel, string name)
        {
            List<Category> categories = await HostedAppService._categoryCollection.Find(x => x.E_Channel == e_Channel && x.Display_name.Contains(name)).ToListAsync();
 
            return categories;
        }
    }
}
