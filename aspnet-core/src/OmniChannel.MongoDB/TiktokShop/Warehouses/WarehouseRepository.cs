using OmniChannel.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace OmniChannel.TiktokShop.Warehouses
{
    public class WarehouseRepository : MongoDbRepository<OmniChannelMongoDbContext, Warehouse, Guid>, IWarehouseRepository
    {
        public WarehouseRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
