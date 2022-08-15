using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.Brands
{
    public class Brand : AuditedAggregateRoot<string>
    {
        public string Name { get; set; }

        private Brand()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal Brand(
               string id,
               [NotNull] string name
           )
           : base(id)
        {
           Name = name;

        }
    }
}
