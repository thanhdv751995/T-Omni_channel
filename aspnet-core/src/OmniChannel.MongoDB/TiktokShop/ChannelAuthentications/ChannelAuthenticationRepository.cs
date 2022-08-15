using OmniChannel.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.ChannelAuthentications
{
    public class ChannelAuthenticationRepository : MongoDbRepository<OmniChannelMongoDbContext, ChannelAuthentication, Guid>, IChannelAuthenticationRepository
    {
        public ChannelAuthenticationRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }


        //public async Task<List<ChannelAuthentication>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
        //{
        //    var dbSet = await GetMongoQueryableAsync();
        //    return await dbSet
        //        .WhereIf(
        //            !filter.IsNullOrWhiteSpace(),
        //            channel => (channel.UserId.Contains(filter))
        //         )
        //        .OrderBy(sorting)
        //        .Skip(skipCount)
        //        .Take(maxResultCount)
        //        .ToListAsync();
        //}
    }
}
