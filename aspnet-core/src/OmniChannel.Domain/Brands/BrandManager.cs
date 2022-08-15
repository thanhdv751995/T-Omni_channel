using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.Brands
{
    public class BrandManager : DomainService
    {

        public BrandManager()
        {
        }
        public Brand CreateAsync(
                string id,
               [NotNull] string name
           )
        {
            return new Brand(
               id,
               name

            );
        }
    }
}
