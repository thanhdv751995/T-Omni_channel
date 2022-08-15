using Hangfire;
using Microsoft.Extensions.Configuration;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.General.BackGroundJobs;
using OmniChannel.General.Orders;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Products;
using OmniChannel.Shares;
using OmniChannel.Shopee.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.BackgroundJob
{
    [RemoteService(true)]
    public class BackgroundJobAppService : OmniChannelAppService, IBackgroundJobAppService
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly string ShopDomain = OmniChannelConsts.TiktokShopAuthorizeDomain;
        private readonly string TiktokShopRefreshAccessToken = OmniChannelConsts.TiktokShopRefreshAccessToken;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly string grant_type = "refresh_token";
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly IConfiguration _configuration;

        public BackgroundJobAppService(
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient,
            IChannelAuthenticationRepository channelAuthenticationRepository,
            ChannelAuthenticationAppService channelAuthenticationAppService,
            IConfiguration configuration
            )
        {
            _recurringJobManager = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _configuration = configuration;
        }
        [RemoteService(false)]
        public void AddUpdateRecurringJob<T>(string jobName, Expression<Action<T>> action, string expression)
        {
            _recurringJobManager.AddOrUpdate<T>(jobName, action, expression);
        }
        #region Tiktok-shop

        [RemoteService(false)]
        // Cập nhật sản phẩm từ tiktok-shop -> database
        public void GetListProduct() => AddUpdateRecurringJob<IProductAppService>("Update_product_tiktokShop_Omni", x => x.GetListProductRepeat(), "*/15 * * * *");

        [RemoteService(false)]
        public void DeleteProduct() => AddUpdateRecurringJob<IProductAppService>("Delete_Product_tiktokShop_Omni", x => x.DeleteProductRepeat(), "*/15 * * * *");

        [RemoteService(false)]
        public void UpdateAccessToken() => AddUpdateRecurringJob<IBackgroundJobAppService>("Check_and_refresh_access_token", x => x.RefreshAccessTokenTiktokShop(), "0 10 * * *");// 10h A.M

        [RemoteService(false)]
        // Refresh token và update lại access token tiktok shop
        public async Task RefreshAccessTokenTiktokShop()
        {
            var timeNow = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var listChannel = await _channelAuthenticationAppService.GetListChannelTiktokShop();

            if(listChannel != null)
            {
                foreach(var channel in listChannel)
                {
                    if((long.Parse(channel.Access_token_expire_in)  - 86400) <= timeNow)
                    {
                    RequestRefreshTokenDto refreshTokenDto = new();

                    refreshTokenDto.app_key = _configuration["AppTiktokShopSetting:App_key"];

                    refreshTokenDto.app_secret = _configuration["AppTiktokShopSetting:App_secret"];

                    refreshTokenDto.refresh_token = channel.Refresh_token;

                    refreshTokenDto.grant_type = grant_type;

                    var url = $"{ShopDomain}{TiktokShopRefreshAccessToken}";

                    var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, refreshTokenDto);

                    var result = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseRefreshDto<DataRefreshDto>>(httpResponseMessage);

                    await _channelAuthenticationAppService.UpdateAccessToken(channel.Channel_token, result.data.Access_token, result.data.Access_token_expire_in);
                    }
                }
            }
        }
    
        [RemoteService(false)]
        public void CheckChangeProductTiktokShop()
        {
             _backgroundJobClient.Enqueue<IProductAppService>(x => x.GetListProductRepeat());
        }
        [RemoteService(false)]
        public void ShopLinkedProduct(string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductAppService>(x => x.ShopLinkedProducts(channel_token));
        }
        #endregion

        #region Shopee

        [RemoteService(false)]
        // tự động đồng bộ sản phẩm shopee => database
        public void AutomaticUpdateProductSP() => AddUpdateRecurringJob<IProductSPAppService>("Update_product_shopee_Omni", x => x.AutomaticProductUpdate(), "*/15 * * * *");
        #endregion
        #region ORDER
        [RemoteService(false)]
        // Cập nhật sản phẩm từ tiktok-shop -> database
        public void GetOrderList(EChannel eChannel) => AddUpdateRecurringJob<IGOrdersAppService>("Order_List_Synchronized", x => x.OrderListSynchronized(eChannel), "* * * * *");
        #endregion
    }
}
