using Microsoft.Extensions.DependencyInjection;
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
    public class RefreshTokenSPWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public RefreshTokenSPWorker(AbpAsyncTimer timer,
           IServiceScopeFactory serviceScopeFactory) : base(
           timer,
           serviceScopeFactory)
        {
            //1 giờ
            Timer.Period = 3600000;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var shopeeAppService = WorkerContext.GetService<IShopeeAppService>(workerContext);

            await shopeeAppService.RefreshAccessToken();
        }
    }
}
