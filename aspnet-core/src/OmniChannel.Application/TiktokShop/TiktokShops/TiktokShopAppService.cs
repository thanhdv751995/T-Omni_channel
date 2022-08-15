using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.TiktokShops
{
    [RemoteService(true)]
    public class TiktokShopAppService : OmniChannelAppService
    {
        private readonly string TiktokShopShopDomain = OmniChannelConsts.TiktokShopShopDomain;
        private readonly string TiktokShopAuthorizeDomain = OmniChannelConsts.TiktokShopAuthorizeDomain;
        private readonly IConfiguration _configuration;
        public TiktokShopAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TiktokShopDto GetTiktokShopConsts()
        {
            return new TiktokShopDto()
            {
                TiktokShopAuthorizeDomain = TiktokShopAuthorizeDomain,
                TiktokShopShopDomain = TiktokShopShopDomain,
                app_key = _configuration["AppTiktokShopSetting:App_key"],
                app_secret = _configuration["AppTiktokShopSetting:App_secret"]
            };
        }
    }
}
