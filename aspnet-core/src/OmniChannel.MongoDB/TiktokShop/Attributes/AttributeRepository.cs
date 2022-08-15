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

namespace OmniChannel.TiktokShop.Attributes
{
    public class AttributeRepository : MongoDbRepository<OmniChannelMongoDbContext, Attribute, Guid>, IAttributeRepository
    {
        public AttributeRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<long> GetCountAsync(EChannel eChannel)
        {
            return await HostedAppService._attribueCollection.CountDocumentsAsync(x => x.E_Channel == eChannel);
        }
    }
}
