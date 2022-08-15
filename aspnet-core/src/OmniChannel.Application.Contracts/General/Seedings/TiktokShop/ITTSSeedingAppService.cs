using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.Seedings.TiktokShop
{
    public interface ITTSSeedingAppService: IApplicationService
    {
        Task SeedingCategory();
        Task SeedingAttribute();
        Task WorkerSeedingCategory();
        Task WorkerSeedingAttribute();
    }
}
