using OmniChannel.General.Reverses;
using Volo.Abp.Domain.Services;

namespace OmniChannel.Reverses
{
    public class ReverseManager : DomainService
    {
        public ReverseManager()
        {

        }

        public Reverse CreateAsync(
              GReverseDto reverse
           )
        {
            return new Reverse(
               GuidGenerator.Create(),
               reverse
            );
        }

    }
}
