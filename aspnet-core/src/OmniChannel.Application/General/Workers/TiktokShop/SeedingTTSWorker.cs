using Microsoft.Extensions.DependencyInjection;
using OmniChannel.Categories;
using OmniChannel.Channels;
using OmniChannel.General.Seedings.TiktokShop;
using OmniChannel.General.Workers;
using OmniChannel.TiktokShop.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace OmniChannel.General.Hosted
{
    public class SeedingTTSWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public SeedingTTSWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(
            timer,
            serviceScopeFactory)
        {
            //1 ngày
            Timer.Period = 86400000;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var seedingAppService = WorkerContext.GetService<ITTSSeedingAppService>(workerContext);

            //Do the work
            await seedingAppService.WorkerSeedingCategory();
            await seedingAppService.WorkerSeedingAttribute();
        }
    }
}
