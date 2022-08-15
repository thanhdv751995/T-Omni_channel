using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using OmniChannel.Authentications;
using OmniChannel.BackgroundJob;
using OmniChannel.Channels;
using OmniChannel.General.Authenticatios;
using OmniChannel.General.ChannelAuthentications;
using OmniChannel.General.Shops;
using OmniChannel.Logistics;
using OmniChannel.Products;
using OmniChannel.Shares;
using OmniChannel.Shops;
using OmniChannel.TiktokShop.ChannelAuthentications;
using OmniChannel.TiktokShop.Signal;
using OmniChannel.TiktokShop.Warehouses;
using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.ChannelAuthentications
{
    // [RemoteService(false)]
    public class ChannelAuthenticationAppService : OmniChannelAppService
    {
        private readonly ChannelAuthenticationManager _channelAuthenticationManager;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly LogisticAppService _logisticAppService;
        public readonly IWarehouseRepository _warehouseRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<SignalR> _hub;
        private readonly AuthenticationAppService _authenticationAppService;
        private readonly ShopAppService _shopAppService;
        public ChannelAuthenticationAppService(ChannelAuthenticationManager channelAuthenticationManager,
           IChannelAuthenticationRepository channelAuthenticationRepository,
           IWarehouseRepository warehouseRepository,
           IHubContext<SignalR> hub,
           IBackgroundJobClient backgroundJobClient,
           LogisticAppService logisticAppService,
           IConfiguration configuration,
           AuthenticationAppService authenticationAppService,
           ShopAppService shopAppService)
        {
            _channelAuthenticationManager = channelAuthenticationManager;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _warehouseRepository = warehouseRepository;
            _logisticAppService = logisticAppService;
            _backgroundJobClient = backgroundJobClient;
            //  _backgroundJobAppService = backgroundJobAppService;
            _configuration = configuration;
            _hub = hub;
            _authenticationAppService = authenticationAppService;
            _shopAppService = shopAppService;
        }

        /// <summary>
        /// connect to tiktok-shop and save data in database - front end redirect to and get authen
        /// </summary>
        /// <param name="channelAuthenticationDto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<ChennelAuthenMessage> CreateChannelAuthentication(GCreateAuthenticationDto gCreateAuthenticationDto)
        {
            RequestGetAccessTokenDto requestGetAccessTokenDto = ObjectMapper.Map<GCreateAuthenticationDto, RequestGetAccessTokenDto>(gCreateAuthenticationDto);

            var accessToken = await _authenticationAppService.CreateAccessTokenWithAuthCode(requestGetAccessTokenDto);

            CreateUpdateChannelAuthenticationDto createUpdateChannelAuthenticationDto = ObjectMapper.Map<DataReponseGetAccessTokenDto, CreateUpdateChannelAuthenticationDto>(accessToken.data);

            createUpdateChannelAuthenticationDto.App = gCreateAuthenticationDto.App;

            createUpdateChannelAuthenticationDto.Client_id = gCreateAuthenticationDto.Client_id;

            var shopInfo = await _shopAppService.GetAuthorizedShop(createUpdateChannelAuthenticationDto.Access_token);

            createUpdateChannelAuthenticationDto.Shop_id = shopInfo.Data.shop_list[0].shop_id;

            var WarehouseList = await _logisticAppService.GetListWareHouseByChannelToken(createUpdateChannelAuthenticationDto.Access_token, createUpdateChannelAuthenticationDto.Shop_id);

            createUpdateChannelAuthenticationDto.Warehouses = WarehouseList;

            if (await _channelAuthenticationRepository.FindAsync(x => x.Shop_id == createUpdateChannelAuthenticationDto.Shop_id) == null)
            {
                //nếu chưa có kết nối
                try
                {
                    //tạo channel_token khi nhận được app và client_id từ tpos
                    var channelToken = CreateChannelToken(createUpdateChannelAuthenticationDto.App, createUpdateChannelAuthenticationDto.Client_id,
                        createUpdateChannelAuthenticationDto.Shop_id, createUpdateChannelAuthenticationDto.Access_token);

                    var channelAuthentication = _channelAuthenticationManager.CreateAsync(channelToken,
                       createUpdateChannelAuthenticationDto.Access_token, createUpdateChannelAuthenticationDto.Access_token_expire_in,
                       createUpdateChannelAuthenticationDto.Refresh_token, createUpdateChannelAuthenticationDto.Refresh_token_expire_in, createUpdateChannelAuthenticationDto.Open_id,
                       createUpdateChannelAuthenticationDto.Seller_name, true, createUpdateChannelAuthenticationDto.Shop_id, createUpdateChannelAuthenticationDto.App, createUpdateChannelAuthenticationDto.Client_id,
                       gCreateAuthenticationDto.E_Channel, createUpdateChannelAuthenticationDto.Warehouses);

                    await _channelAuthenticationRepository.InsertAsync(channelAuthentication);



                    _backgroundJobClient.Enqueue<IProductAppService>(x => x.ShopLinkedProducts(channelToken));

                    // bắn signal khi tạo thành công 
                    await _hub.Clients.All.SendAsync("ReceiveMessage", "Thêm kết nối thành công");

                    GShopDto shopDto = new()
                    {
                        E_Channel = createUpdateChannelAuthenticationDto.E_Channel,
                        Shop_id = createUpdateChannelAuthenticationDto.Shop_id,
                        Shop_name = createUpdateChannelAuthenticationDto.Seller_name,
                        Last_connected_time = channelAuthentication.CreationTime,
                        Channel_token = channelAuthentication.Channel_token,
                        Is_active = true
                    };

                    return new ChennelAuthenMessage()
                    {
                        Message = "Thêm kết nối thành công",
                        Channel_token = channelToken,
                        Shop_data = shopDto
                    };
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("Something wrong - " + e.Message);
                }

            }
            else
            {
                //nếu đã có kết nối thì update lại accessToken
                try
                {
                    var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Shop_id == createUpdateChannelAuthenticationDto.Shop_id);


                    channelAuthen.Access_token = createUpdateChannelAuthenticationDto.Access_token;

                    channelAuthen.Access_token_expire_in = createUpdateChannelAuthenticationDto.Access_token_expire_in;

                    channelAuthen.Refresh_token = createUpdateChannelAuthenticationDto.Refresh_token;

                    channelAuthen.Refresh_token_expire_in = createUpdateChannelAuthenticationDto.Refresh_token_expire_in;

                    channelAuthen.Warehouse_list = createUpdateChannelAuthenticationDto.Warehouses;

                    channelAuthen.CreationTime = DateTime.Now;

                    await _channelAuthenticationRepository.UpdateAsync(channelAuthen);

                    _backgroundJobClient.Enqueue<IProductAppService>(x => x.ShopLinkedProducts(channelAuthen.Channel_token));

                    // bắn signal khi update thành công 

                    await _hub.Clients.All.SendAsync("ReceiveMessage", "Shop này đã được kết nối trước đó");

                    GShopDto shopDto = new()
                    {
                        E_Channel = createUpdateChannelAuthenticationDto.E_Channel,
                        Shop_id = createUpdateChannelAuthenticationDto.Shop_id,
                        Shop_name = createUpdateChannelAuthenticationDto.Seller_name,
                        Last_connected_time = channelAuthen.CreationTime,
                        Channel_token = channelAuthen.Channel_token,
                        Is_active = true
                    };

                    return new ChennelAuthenMessage()
                    {
                        Message = "Shop này đã được kết nối trước đó",
                        Channel_token = channelAuthen.Channel_token,
                        Shop_data = shopDto
                    };
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("Something wrong - " + e.Message);
                }
            }
        }

        [RemoteService(false)]
        public async Task<List<ChannelAuthenticationDto>> GetListChannelAuthentication(string channel_token)
        {
            var listChannelAuthen = await _channelAuthenticationRepository.GetListAsync(x => x.Channel_token == channel_token);
            return ObjectMapper.Map<List<ChannelAuthentication>, List<ChannelAuthenticationDto>>(listChannelAuthen);
        }

        #region Shopee

        public async Task<List<ChannelAuthenticationDto>> GetListChannelShopee()
        {
            var listChannelAuthen = await _channelAuthenticationRepository.GetListAsync(x => x.E_Channel == EChannel.Shopee);
            return ObjectMapper.Map<List<ChannelAuthentication>, List<ChannelAuthenticationDto>>(listChannelAuthen);
        }

        public async Task CreateChannelAuthenticationShopee(CreateChannelAuthenticationDto createChannelAuthenticationDto)
        {
            var currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            // thời gian hiện tại + time expire (4 tiếng)
            createChannelAuthenticationDto.Access_token_expire_in += currentTime;

            // kiểm tra shop đã tồn tại chưa
            bool isShopExist = await _channelAuthenticationRepository.AnyAsync(x => x.Shop_id == createChannelAuthenticationDto.Shop_id
            && x.E_Channel == createChannelAuthenticationDto.E_channel);

            //nếu shop chưa có trong database thì tạo
            if (!isShopExist)
            {
                var channelToken = CreateChannelToken(createChannelAuthenticationDto.App,
                    createChannelAuthenticationDto.Client_id,
                    createChannelAuthenticationDto.Shop_id,
                    createChannelAuthenticationDto.Access_token);

                var channelAuthentication = _channelAuthenticationManager.CreateAsync(channelToken,
                           createChannelAuthenticationDto.Access_token,
                           createChannelAuthenticationDto.Access_token_expire_in,
                           createChannelAuthenticationDto.Refresh_token,
                           createChannelAuthenticationDto.Refresh_token_expire_in,
                           createChannelAuthenticationDto.Open_id,
                           createChannelAuthenticationDto.Seller_name,
                           true,
                           createChannelAuthenticationDto.Shop_id,
                           createChannelAuthenticationDto.App,
                           createChannelAuthenticationDto.Client_id,
                           createChannelAuthenticationDto.E_channel,
                           null);

                await _channelAuthenticationRepository.InsertAsync(channelAuthentication);
            }
            // shop có rồi thì update
            else
            {
                var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Shop_id == createChannelAuthenticationDto.Shop_id
                && x.E_Channel == createChannelAuthenticationDto.E_channel);

                channelAuthen.Access_token = createChannelAuthenticationDto.Access_token;
                channelAuthen.Access_token_expire_in = createChannelAuthenticationDto.Access_token_expire_in;
                channelAuthen.Refresh_token = createChannelAuthenticationDto.Refresh_token;
                channelAuthen.Refresh_token_expire_in = createChannelAuthenticationDto.Refresh_token_expire_in;
                channelAuthen.Seller_name = createChannelAuthenticationDto.Seller_name;
                channelAuthen.App = createChannelAuthenticationDto.App;
                channelAuthen.Client_id = createChannelAuthenticationDto.Client_id;
                channelAuthen.Open_id = createChannelAuthenticationDto.Open_id;
                channelAuthen.E_Channel = createChannelAuthenticationDto.E_channel;
                channelAuthen.CreationTime = DateTime.Now;

                await _channelAuthenticationRepository.UpdateAsync(channelAuthen);
            }
        }

        #endregion

        public async Task RefreshUpdateChannelToken(CreateChannelAuthenticationDto createChannelAuthenticationDto)
        {
            var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Shop_id == createChannelAuthenticationDto.Shop_id);

            if (channelAuthen != null)
            {
                channelAuthen.Access_token = createChannelAuthenticationDto.Access_token;

                channelAuthen.Access_token_expire_in = createChannelAuthenticationDto.Access_token_expire_in;

                channelAuthen.Refresh_token = createChannelAuthenticationDto.Refresh_token;

                channelAuthen.Refresh_token_expire_in = createChannelAuthenticationDto.Refresh_token_expire_in;

                channelAuthen.CreationTime = DateTime.Now;

                await _channelAuthenticationRepository.UpdateAsync(channelAuthen);
            }

        }
        public async Task<ChannelAuthenticationDto> GetChannelAuthenticationById(string channel_token)
        {
            var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            return ObjectMapper.Map<ChannelAuthentication, ChannelAuthenticationDto>(channelAuthen);
        }

        public async Task DeleteAsync(string channel_token)
        {
            if (await _channelAuthenticationRepository.AnyAsync(x => x.Channel_token == channel_token) == false)
            {
                throw new BusinessException("Channel_token không tồn tại");
            }
            else
            {
                await _channelAuthenticationRepository.DeleteAsync(x => x.Channel_token == channel_token);
            }
        }

        public async Task UpdateAsync(string channel_token, List<WarehouseDto> warehouseDtos)
        {
            var shopAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            shopAuthen.Warehouse_list = warehouseDtos;

            shopAuthen.CreationTime = DateTime.Now;

            shopAuthen.IsActive = !shopAuthen.IsActive;

            await _channelAuthenticationRepository.UpdateAsync(shopAuthen);
        }

        /// <summary>
        /// Create channel_token including app , client id to sha256 from tpos
        /// </summary>
        /// <param name="app"></param>
        /// <param name="clientId"></param>
        /// <param name="shopId"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string CreateChannelToken(string app, string clientId, string shopId, string access_token)
        {
            //tạo mã channel_token bằng sha256
            var result = ShareAppService.CalcHMACSHA256Hash($"{app}{shopId}{clientId}{access_token}{ShareAppService.GetTimestamp()}", _configuration["Secret:SecretKey"]);

            return result;
        }

        public async Task<ChannelAuthentication> GetFirstAuthentication(EChannel e_channel)
        {
            return await _channelAuthenticationRepository.FirstOrDefaultAsync(x => x.E_Channel == e_channel);
        }

        public async Task<string> GetWarehouseIdAuthentication(string channel_token, int type)
        {
            var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            return channelAuthen.Warehouse_list.FirstOrDefault(x => x.Warehouse_type == type).Warehouse_id;
        }
        public async Task<List<ChannelAuthenticationDto>> GetListChannelTiktokShop()
        {
            var listChannel = await _channelAuthenticationRepository.GetListAsync(x => x.E_Channel == EChannel.TiktokShop);
            return ObjectMapper.Map<List<ChannelAuthentication>, List<ChannelAuthenticationDto>>(listChannel);
        }

        // update access token from background job
        public async Task UpdateAccessToken(string channel_token, string accessToken, string AccessTokenExpire)
        {
            var channelAuthen = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            channelAuthen.Access_token = accessToken;

            channelAuthen.Access_token_expire_in = AccessTokenExpire;

            await _channelAuthenticationRepository.UpdateAsync(channelAuthen);
        }

        public async Task<EChannel> GetEChannel(string channel_token)
        {
            var channel_authentication = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            return channel_authentication.E_Channel;
        }
    }

}
