using OmniChannel.TiktokShop.WebHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.TiktokShop.Webhooks
{
    [RemoteService(false)]
    public class WebhookAppService : OmniChannelAppService
    {
        public WebhookAppService()
        {
        }

        public string PushNotifiFromTiktok(string test)
        {
            return test;
        }
        //public string GetAsync(string dto)
        //{
        //    return dto;
        //}    
    }
}
