using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OmniChannel.Finances.Settlements;
using OmniChannel.Finances.Transactions;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using Volo.Abp;

namespace OmniChannel.Finances
{
    [RemoteService(true)]
    public class FinanceAppService: OmniChannelAppService
    {
        private readonly ShareAppService _shareAppService;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly string TiktokShopGetSettlements = OmniChannelConsts.TiktokShopGetSettlements;
        private readonly string TiktokShopGetTransactions = OmniChannelConsts.TiktokShopGetTransactions;
        private readonly string TiktokShopGetOrderSettlements = OmniChannelConsts.TiktokShopGetOrderSettlements;
        private readonly IConfiguration _configuration;
        public FinanceAppService(ShareAppService shareAppService, IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }
        [HttpPost]
        /// <summary>
        /// get settlements
        /// </summary>
        /// <param name="requestGetSettlementsDto"></param>
        /// <param name="channel_token"> channel_token from channel authentication collection</param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseGetSettlementsDto>> GetSettlements(RequestGetSettlementsDto requestGetSettlementsDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetSettlements, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestGetSettlementsDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetSettlementsDto>>(httpResponseMessage);
        }
        [HttpPost]
        /// <summary>
        /// get transactions
        /// </summary>
        /// <param name="requestGetTransactionsDto"></param>
        /// <param name="channel_token">channel_token from channel authentication collection</param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseGetTransactionsDto>> GetTransactions(RequestGetTransactionsDto requestGetTransactionsDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetTransactions, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestGetTransactionsDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetTransactionsDto>>(httpResponseMessage);
        }
        [HttpGet]
        /// <summary>
        /// get orrder settlements
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseGetOrderSettlementsDto>> OrderSettlements(string order_id, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"order_id={order_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetOrderSettlements, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, requestParameters);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetOrderSettlementsDto>>(httpResponseMessage);
        }
    }
}
