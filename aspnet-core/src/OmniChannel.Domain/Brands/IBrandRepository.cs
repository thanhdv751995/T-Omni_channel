using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.Brands
{
    public interface IBrandRepository : IRepository<Brand, string>
    {
    }
}
