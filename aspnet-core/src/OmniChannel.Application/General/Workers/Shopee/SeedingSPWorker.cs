using Microsoft.Extensions.DependencyInjection;
using OmniChannel.General.Seedings.Shopee;
using OmniChannel.Shopee.Shopees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace OmniChannel.General.Workers.Shopee
{
    public class SeedingSPWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public SeedingSPWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(
            timer,
            serviceScopeFactory)
        {
            //1 ngày
            Timer.Period = 86400000;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var seedingAppService = WorkerContext.GetService<ISPSeedingAppService>(workerContext);
            var shopeeAppService = WorkerContext.GetService<IShopeeAppService>(workerContext);

            await seedingAppService.WorkerSeedingCategory();
            await seedingAppService.WorkerSeedingAttribute();
            await shopeeAppService.RefreshAccessToken();
        }
    }
}
