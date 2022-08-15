using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.ChannelAuthentications
{
    public interface IChannelAuthenticationRepository : IRepository<ChannelAuthentication, Guid>
    {
        //   Task<List<ChannelAuthentication>> GetListAsync(
        //    int skipCount,
        //    int maxResultCount,
        //    string sorting,
        //    string filter = null
        //);
    }
}
