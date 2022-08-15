using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.Products
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Tuple<int, List<Product>>> GetListPagedAsync(List<string> listShopId,EChannel eChannel ,int skip, int take, string shopId, bool? is_Linked);
    }
}
