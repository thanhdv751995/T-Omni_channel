using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.Categories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<List<Category>> GetListAsync(EChannel e_channel, string filter = "");
        Task<long> GetCountAsync(EChannel e_channel);
    }
}
