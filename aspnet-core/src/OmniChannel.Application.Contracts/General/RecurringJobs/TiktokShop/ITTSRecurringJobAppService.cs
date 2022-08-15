using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.General.RecurringJobs.TiktokShop
{
    public interface ITTSRecurringJobAppService
    {
        Task UpdateSkuIdIfNull(double days);
    }
}
