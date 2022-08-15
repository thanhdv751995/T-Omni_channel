using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Authentications
{
    [RemoteService(false)]
    public class AuthenticationAppService : OmniChannelAppService
    {
        private readonly string TiktokShopAuthorizeDomain = OmniChannelConsts.TiktokShopAuthorizeDomain;
        private readonly string TiktokShopGetAccessToken = OmniChannelConsts.TiktokShopGetAccessToken;
        public AuthenticationAppService()
        {

        }
        /// <summary>
        /// Generate access_token with auth_code
        /// </summary>
        /// <param name="requestGetAccessTokenDto"></param>
        /// <returns></returns>
        public async Task<ReponseGetAuthenDto> CreateAccessTokenWithAuthCode(RequestGetAccessTokenDto requestGetAccessTokenDto)
        {
            var url = TiktokShopAuthorizeDomain + TiktokShopGetAccessToken;

            HttpResponseMessage response = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestGetAccessTokenDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ReponseGetAuthenDto>(response);
        }
    }
}
