using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;

namespace OmniChannel.General.Workers
{
    public class WorkerContext
    {
        public WorkerContext()
        {

        }

        public static T GetService<T>(PeriodicBackgroundWorkerContext workerContext)
        {
            return workerContext
            .ServiceProvider
            .GetRequiredService<T>();
        }
    }
}
