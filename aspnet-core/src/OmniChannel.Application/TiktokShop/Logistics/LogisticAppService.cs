using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OmniChannel.Deliveries;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using OmniChannel.Ships;
using OmniChannel.Warehouses;
using Volo.Abp;

namespace OmniChannel.Logistics
{
    [RemoteService(true)]
    public class LogisticAppService : OmniChannelAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly string TiktokShopGetWarehouseList = OmniChannelConsts.TiktokShopGetWarehouseList;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly string TiktokShopGetShippingInfo = OmniChannelConsts.TiktokShopGetShippingInfo;
        private readonly string TiktokShopGetShippingProvider = OmniChannelConsts.TiktokShopGetShippingProvider;
        private readonly string TiktokShopGetShippingDocument = OmniChannelConsts.TiktokShopGetShippingDocument;
        private readonly string TiktokShopGetSubscribedDeliveryOptions = OmniChannelConsts.TiktokShopGetSubscribedDeliveryOptions;
        private readonly string TiktokShopUpdateShippingInfo = OmniChannelConsts.TiktokShopUpdateShippingInfo;
        private readonly IConfiguration _configuration;
        public LogisticAppService(ShareAppService shareAppService , IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }

        public async Task<ResponseDataDto<ResponseWarehouseDto>> GetWarehouseList(string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetWarehouseList, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseWarehouseDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataDto<ResponseTrackingInfoDto>> GetShippingInfo(string order_id , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"order_id={order_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetShippingInfo, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token, channel.Shop_id,
                _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseTrackingInfoDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataDto<ResponseDeliveryOptionDto>> GetShippingProvider(string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetShippingProvider, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseDeliveryOptionDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataDto<ResponseShippingDocumentDto>> GetShippingDocument(string order_id, string document_type, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"order_id={order_id}", $"document_type={document_type}", $"document_size=A6");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetShippingDocument, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseShippingDocumentDto>>(httpResponseMessage);
        }

        public async Task<string> SubscribedDeliveryOptions(RequestWarehouseIdListDto requestWarehouseIdListDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetSubscribedDeliveryOptions, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestWarehouseIdListDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostUpdateShippingInfo(RequestUpdateShippingInfoDto requestUpdateShippingInfoDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopUpdateShippingInfo, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestUpdateShippingInfoDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        public async Task<List<WarehouseDto>> GetListWareHouseByChannelToken( string access_token, string shop_id)
        {
            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetWarehouseList, _configuration["AppTiktokShopSetting:App_key"], access_token,
                shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            var result = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseWarehouseDto>>(httpResponseMessage);
            return result.Data.warehouse_list;
        }
    }
}
