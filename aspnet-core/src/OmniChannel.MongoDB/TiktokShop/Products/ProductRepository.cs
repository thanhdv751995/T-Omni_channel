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

namespace OmniChannel.Products
{
    public class ProductRepository : MongoDbRepository<OmniChannelMongoDbContext, Product, Guid>, IProductRepository
    {
        public ProductRepository(IMongoDbContextProvider<OmniChannelMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        [Obsolete]
        public async Task<Tuple<int, List<Product>>> GetListPagedAsync(List<string> listShopId,
            EChannel eChannel,
            int skip,
            int take,
            string shopId,
            bool? is_Linked = null)
        {
            if (shopId.IsNullOrWhiteSpace())
            {
                shopId = "";
            }

            List<Product> products = await HostedAppService._productCollection.Find(x =>!x.IsDeleted && listShopId.Contains(x.Shop_Id)
            && x.Shop_Id.Contains(shopId) && x.E_Channel == eChannel && (is_Linked == null || x.IsLinked == is_Linked)).ToListAsync();

            return new Tuple<int, List<Product>>(
                products.Count,
                products.Skip(skip).Take(take).OrderByDescending(x=>x.CreationTime).ToList()
                );
        }
    }
}
