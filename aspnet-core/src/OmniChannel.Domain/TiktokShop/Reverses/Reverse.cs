using OmniChannel.General.Reverses;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.Reverses
{
    public class Reverse : AuditedAggregateRoot<string>
    {
        public GReverseDto ReverseDto { get; set; }

        private Reverse()
        {
            /* This constructor is for deserialization / ORM purpose */
        }

        internal Reverse(
            Guid id,
            GReverseDto reverse
        ) : base(id.ToString())
        {
            ReverseDto = reverse;
        }
    }
}
