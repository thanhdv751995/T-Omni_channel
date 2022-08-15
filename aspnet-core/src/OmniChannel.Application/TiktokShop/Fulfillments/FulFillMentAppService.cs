using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OmniChannel.FulFillments;
using OmniChannel.FulFillments.Package;
using OmniChannel.FulFillments.Split;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using Volo.Abp;

namespace OmniChannel.FulFillMents
{
    [RemoteService(true)]
    public class FulFillMentAppService : OmniChannelAppService
    {

        private readonly ShareAppService _shareAppService;
        //private readonly string shopId = OmniChannelConsts.shopId;
        //private readonly string access_token = OmniChannelConsts.access_token;
        private readonly string TiktokShopApiGetVerifyOrderSplit = OmniChannelConsts.TiktokShopApiGetVerifyOrderSplit;
        private readonly string TiktokShopConfirmOrderSplit = OmniChannelConsts.TiktokShopConfirmOrderSplit;
        private readonly string TiktokShopApiGetSearchPreCombinePkg = OmniChannelConsts.TiktokShopApiGetSearchPreCombinePkg;
        private readonly string TiktokShopGetPackageShippingDocument = OmniChannelConsts.TiktokShopGetPackageShippingDocument;
        private readonly string TiktokShopUpdatePackageShippingInfo = OmniChannelConsts.TiktokShopUpdatePackageShippingInfo;
        private readonly string TiktokShopGetPackageShippingInfo = OmniChannelConsts.TiktokShopGetPackageShippingInfo;
        private readonly string TiktokShopSearchPackage = OmniChannelConsts.TiktokShopSearchPackage;
        private readonly string TiktokShopShipPackage = OmniChannelConsts.TiktokShopShipPackage;
        private readonly string TiktokShopGetPackagePickupConfig = OmniChannelConsts.TiktokShopGetPackagePickupConfig;
        private readonly string TiktokShopRemovePackageOrder = OmniChannelConsts.TiktokShopRemovePackageOrder;
        private readonly string TiktokShopConfirmPrecombinePackage = OmniChannelConsts.TiktokShopConfirmPrecombinePackage;
        private readonly string TiktokShopGetPackageDetail = OmniChannelConsts.TiktokShopGetPackageDetail;
        private readonly string TiktokShopUpdatePackageDeliveryStatus = OmniChannelConsts.TiktokShopUpdatePackageDeliveryStatus;
        private readonly IConfiguration _configuration;

        public FulFillMentAppService(ShareAppService shareAppService , IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _configuration = configuration;
        }
        public async Task<string> GetSearchPreCombinePkg(int? page_size, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"page_size={page_size}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetSearchPreCombinePkg, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        public async Task<ResponseDataDto<ResponseGetPackageShippingDocumentDto>> GetPackageShippingDocument(int package_id, int document_type,
            int? document_size, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"package_id={package_id}", $"document_type={document_type}", $"document_size={document_size}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetPackageShippingDocument, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetPackageShippingDocumentDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataDto<ResponseTrackingInfoListDto>> GetPackageShippingInfo(string package_id, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"package_id={package_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetPackageShippingInfo, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseTrackingInfoListDto>>(httpResponseMessage);
        }

        public async Task<string> GetPackagePickupConfig(string package_id , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"package_id={package_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetPackagePickupConfig, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<ResponseDataDto<ResponseGetPackageDetailDto>> GetPackageDetail(string package_id , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"package_id={package_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopGetPackageDetail, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseGetPackageDetailDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataDto<ResponseVerifyOrderSplitDto>> VerifyOrderSplit(RequestListOrderIdDto requestListOrderIdDto  ,string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetVerifyOrderSplit, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestListOrderIdDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseVerifyOrderSplitDto>>(httpResponseMessage);
        }

        public async Task<string> ConfirmOrderSplit(RequestOrderConfirmSplitDto requestOrderConfirmSplitDto , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopConfirmOrderSplit, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestOrderConfirmSplitDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }


        public async Task<string> PostUpdatePackageShippingInfo(RequestUpdatePackageShippingInfoDto requestUpdatePackageShippingInfoDto  , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopUpdatePackageShippingInfo, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestUpdatePackageShippingInfoDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<ResponseDataDto<ResponseSearchPackageDto>> SearchPackage(SearchPackageDto searchPackageDto  , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopSearchPackage, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
               channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, searchPackageDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseSearchPackageDto>>(httpResponseMessage);
        }

        public async Task<string> ShipPackage(RequestShipPackageDto requestShipPackageDto , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopShipPackage, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestShipPackageDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostRemovePackageOrder(RequestRemovePackageOrderDto requestRemovePackageOrderDto  , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopRemovePackageOrder, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestRemovePackageOrderDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> ConfirmPrecombinePackage(RequestCombinePkgListDto requestCombinePkgListDto  , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopConfirmPrecombinePackage, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestCombinePkgListDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostUpdatePackageDeliveryStatus(RequestDeliveryPackagesDto requestDeliveryPackagesDto  , string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopUpdatePackageDeliveryStatus, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestDeliveryPackagesDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
