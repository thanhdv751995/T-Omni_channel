using OmniChannel.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.Orders
{
    public class OrderRepository : MongoDbRepository<OmniChannelMongoDbContext, Order, string>, IOrderRepository
    {
        public OrderRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
