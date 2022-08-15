using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.General.Authenticatios;
using OmniChannel.General.ChannelAuthentications;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using OmniChannel.Shopee.Authentications;
using OmniChannel.Shopee.Shops;
using OmniChannel.Shopee.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Shopee.Shopees
{
   // [RemoteService(false)]
    public class ShopeeAppService : OmniChannelAppService, IShopeeAppService
    {
        private readonly string DOMAIN_URL = ShopeeConst.DOMAIN_URL;
        private readonly string API_AUTH_PARTNER = ShopeeConst.API_AUTH_PARTNER;
        private readonly string CALLBACK_REDIRECT_URL = ShopeeConst.CALLBACK_REDIRECT_URL;
        private readonly string API_GET_TOKEN = ShopeeConst.API_GET_TOKEN;
        private readonly string API_REFRESH_ACCESS_TOKEN = ShopeeConst.API_REFRESH_ACCESS_TOKEN;

        private readonly IConfiguration _configuration;
        private readonly ShareAppService _shareAppService;
        private readonly ShopAppService _shopAppService;
        private readonly WarehouseAppService _warehouseAppService;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;

        public ShopeeAppService(IConfiguration configuration,
            ShareAppService shareAppService,
            ShopAppService shopAppService,
            WarehouseAppService warehouseAppService,
            ChannelAuthenticationAppService channelAuthenticationAppService)
        {
            _configuration = configuration;
            _shareAppService = shareAppService;
            _shopAppService = shopAppService;
            _warehouseAppService = warehouseAppService;
            _channelAuthenticationAppService = channelAuthenticationAppService;
        }

        public AuthenticationUrlDto GetAuthorizeUrl()
        {
            var sign = _shareAppService.GetSignatureAlgorithmShopee(API_AUTH_PARTNER, 0, string.Empty, true);

            var authorizeUrl = $"{DOMAIN_URL}{API_AUTH_PARTNER}" +
                $"?partner_id={_configuration["ShopeeSetting:Partner_id"]}" +
                $"&redirect={CALLBACK_REDIRECT_URL}" +
                $"&sign={sign}" +
                $"&timestamp={ShareAppService.GetTimestamp()}";

            return new AuthenticationUrlDto { AuthorizeUrl = authorizeUrl };
        }

        public async Task<ChannelAuthenticationShopeeDto> AccessToken(long shop_id, string code)
        {
            var sign = _shareAppService.GetSignatureAlgorithmShopee(API_GET_TOKEN, 0, string.Empty, true);

            var authorizeUrl = $"{DOMAIN_URL}{API_GET_TOKEN}" +
                $"?partner_id={int.Parse(_configuration["ShopeeSetting:Partner_id"])}" +
                $"&timestamp={ShareAppService.GetTimestamp()}" +
                $"&sign={sign}";

            RequestGetAccessTokenShopeeDto requestGetAccessTokenShopeeDto = new()
            {
                code = code,
                partner_id = int.Parse(_configuration["ShopeeSetting:Partner_id"]),
                shop_id = shop_id
            };

            var httpResponseMessageGetToken = await HttpClientAppService.GetResponseMessage(authorizeUrl, EHttpMethod.POST, requestGetAccessTokenShopeeDto);

            var responseGetAccessToken = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseGetAccessTokenDto>(httpResponseMessageGetToken);

            var shopInfo = await _shopAppService.GetShopInfo(shop_id, responseGetAccessToken.access_token);

            var responseGetShopInfo = _shareAppService.ConvertStringToDto<ResponseShopInfoDto>(shopInfo);

            //var WarehouseDetail = _warehouseAppService.GetWarehouseDetail(shop_id, responseGetAccessToken.access_token);

            return new ChannelAuthenticationShopeeDto()
            {
                access_token = responseGetAccessToken.access_token,
                access_token_expire_in = responseGetAccessToken.expire_in,
                refresh_token = responseGetAccessToken.refresh_token,
                refresh_token_expire_in = responseGetAccessToken.refresh_token_expire_in,
                open_id = shop_id,
                shop_id = shop_id,
                e_channel = EChannel.Shopee,
                seller_name = responseGetShopInfo.shop_name
            };
        }
        /// <summary>
        /// refresh access_token shoppee all shop 
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <param name="shop_id"></param>
        /// <returns></returns>
        public async Task RefreshAccessToken()
        {
            var listChannel = await _channelAuthenticationAppService.GetListChannelShopee();

            var timeNow = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var timeExpire = timeNow + 14377;
            if (listChannel!= null)
            {
                foreach (var channel in listChannel)
                {
                    var timeReset = timeNow - 3594;// kiểm tra thời gian hết hạn <= 1 giờ tới thời gian hiện tại thì reset

                    if (int.Parse(channel.Access_token_expire_in)  >= timeReset)
                    {
                        var sign = _shareAppService.GetSignatureAlgorithmShopee(API_REFRESH_ACCESS_TOKEN, 0, string.Empty, true);
                        var authorizeUrl = $"{DOMAIN_URL}{API_REFRESH_ACCESS_TOKEN}" +
                          $"?sign={sign}" +
                          $"&partner_id={_configuration["ShopeeSetting:Partner_id"]}" +
                          $"&timestamp={ShareAppService.GetTimestamp()}";
                        RequestRefreshAccessTokenShopeeDTo requestRefreshAccessTokenShopeeDTo = new()
                        {
                            Refresh_token = channel.Refresh_token,
                            Partner_id = int.Parse(_configuration["ShopeeSetting:Partner_id"]),
                            Shop_id = int.Parse(channel.Shop_id)
                        };
                        var httpResponseMessageRefreshToken = await HttpClientAppService.GetResponseMessage(authorizeUrl, EHttpMethod.POST, requestRefreshAccessTokenShopeeDTo);
                        var result = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ReponseRefreshTokenShopeeDto>(httpResponseMessageRefreshToken);
                        CreateChannelAuthenticationDto createChannelAuthenticationDto = new()
                        {
                            Access_token = result.access_token,
                            Access_token_expire_in = timeExpire.ToString(),
                            Refresh_token = result.refresh_token,
                            Refresh_token_expire_in = "0",
                        };

                        await _channelAuthenticationAppService.RefreshUpdateChannelToken(createChannelAuthenticationDto);
                    }
                   
                }
            }

           
        }
    }
}
