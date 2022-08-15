using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.TiktokShop.Attributes
{
    public interface IAttributeRepository : IRepository<Attribute, Guid>
    {
        Task<long> GetCountAsync(EChannel eChannel);
    }
}
