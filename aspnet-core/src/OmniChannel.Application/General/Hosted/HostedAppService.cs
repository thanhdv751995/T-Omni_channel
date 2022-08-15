using Hangfire;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using OmniChannel.Attributes;
using OmniChannel.BackgroundJob;
using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.General.Connects;
using OmniChannel.General.Orders;
using OmniChannel.General.Seedings.Shopee;
using OmniChannel.General.Seedings.TiktokShop;
using OmniChannel.Products;
using OmniChannel.TiktokShop.Attributes;
using OmniChannel.TiktokShop.Categories;
using OmniChannel.TiktokShop.ProductImages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniChannel.TiktokShop.Hosted
{
    public class HostedAppService : IHostedService
    {
        private readonly TTSSeedingAppService _tTSSeedingAppService;
        private readonly ConnectMongoDBAppService _connectDBAppService;
        private readonly AttributeAppService _attributeAppService;
        private readonly BackgroundJobAppService _backgroundJobAppService;
        private readonly SPSeedingAppService _sPSeedingAppService;
        private readonly IGOrdersAppService _gOrdersAppService;

        public static IMongoCollection<Category> _categoryCollection;
        public static IMongoCollection<Attribute> _attribueCollection;
        public static IMongoCollection<Product> _productCollection;
        public static IMongoCollection<ProductImage> _productImageCollection;
        private static IMongoDatabase mongoDatabase = null;

        public HostedAppService(TTSSeedingAppService tTSSeedingAppService,
            ConnectMongoDBAppService connectDBAppService,
            AttributeAppService attributeAppService,
            BackgroundJobAppService backgroundJobAppService,
            SPSeedingAppService sPSeedingAppService,
            IGOrdersAppService gOrdersAppService)
        {
            _tTSSeedingAppService = tTSSeedingAppService;
            _connectDBAppService = connectDBAppService;
            _attributeAppService = attributeAppService;
            _backgroundJobAppService = backgroundJobAppService;
            _sPSeedingAppService = sPSeedingAppService;
            _gOrdersAppService = gOrdersAppService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            mongoDatabase = _connectDBAppService.ConnectDB();

            _categoryCollection = GetMongoCollection<Category>();
            _attribueCollection = GetMongoCollection<Attribute>();
            _productCollection = GetMongoCollection<Product>();
            _productImageCollection = GetMongoCollection<ProductImage>();

            #region Seeding

            //tiktok shop
            //await _tTSSeedingAppService.SeedingCategory();
            //await _tTSSeedingAppService.SeedingAttribute();

            //shopee
            //await _sPSeedingAppService.SeedingCategory();
            //await _sPSeedingAppService.SeedingAttribute();
            #endregion
            #region Tiktok-shop
            _backgroundJobAppService.GetListProduct();
            _backgroundJobAppService.DeleteProduct();
            _backgroundJobAppService.UpdateAccessToken();
            #endregion

            #region Shopee
            _backgroundJobAppService.AutomaticUpdateProductSP();

            #endregion

            #region ORDER: Lấy danh sách đơn hàng từ TikTok shop
            _backgroundJobAppService.GetOrderList(EChannel.TiktokShop);
            #endregion
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static IMongoCollection<T> GetMongoCollection<T>()
        {
            return mongoDatabase.GetCollection<T>(typeof(T).Name);
        }
    }
}
