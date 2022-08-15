using Microsoft.Extensions.DependencyInjection;
using OmniChannel.General.RecurringJobs.TiktokShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace OmniChannel.General.Workers.TiktokShop
{
    public class RecurringJobTTSWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public RecurringJobTTSWorker(AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(
            timer,
            serviceScopeFactory)
        {
            //1 ngày
            Timer.Period = 86400000;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var recurringJobTTSAppService = WorkerContext.GetService<ITTSRecurringJobAppService>(workerContext);

            //Do the work
            await recurringJobTTSAppService.UpdateSkuIdIfNull(-2);
        }
    }
}
