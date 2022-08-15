using OmniChannel.MongoDB;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.Reverses
{
    public class ReverseRepository : MongoDbRepository<OmniChannelMongoDbContext, Reverse, string>, IReverseRepository
    {
        public ReverseRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
