using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.Seedings.Shopee
{
    public interface ISPSeedingAppService : IApplicationService
    {
        Task SeedingCategory();
        Task SeedingAttribute();
        Task WorkerSeedingCategory();
        Task WorkerSeedingAttribute();
    }
}
