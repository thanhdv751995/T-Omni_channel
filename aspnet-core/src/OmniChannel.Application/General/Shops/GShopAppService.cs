using OmniChannel.Authentications;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Products;
using OmniChannel.Shares;
using OmniChannel.WareHouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace OmniChannel.General.Shops
{
    [RemoteService(false)]
    public class GShopAppService : OmniChannelAppService, IShopAppService
    {
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly IProductRepository _productRepository;
        public GShopAppService(
            IChannelAuthenticationRepository channelAuthenticationRepository,
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _channelAuthenticationRepository = channelAuthenticationRepository;
        }
        /// <summary>
        /// danh sách shop
        /// </summary>
        /// <param name="app"></param>
        /// <param name="client_id"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GShopDto>> GetListShop(string app, string client_id)
        {
            var list_Shop = await _channelAuthenticationRepository.GetListAsync(x => x.App == app && x.Client_id == client_id);

            var result = ObjectMapper.Map<List<ChannelAuthentication>, List<GShopDto>>(list_Shop);

            return new PagedResultDto<GShopDto>(
                list_Shop.Count,
                result
                );
        }
        /// <summary>
        /// Update Linked
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task UpdateLinkedShop(string channel_token)
        {
            var shop = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            shop.IsActive = !shop.IsActive;

            await _channelAuthenticationRepository.UpdateAsync(shop);

            if (!shop.IsActive)
            {
                var listProduct = await _productRepository.GetListAsync(x => x.Shop_Id == shop.Shop_id);

                listProduct.ForEach(x => x.IsLinked = false);

                await _productRepository.UpdateManyAsync(listProduct);
            }           
        }
        /// <summary>
        /// Lấy danh sách chi tiết gian hàng
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ShopDetailDto> ShopDetail(string channel_token)
        {
            var shop = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            var result = ObjectMapper.Map<ChannelAuthentication, ShopDetailDto>(shop);


            result.Warehouse_list.ForEach(e =>
            {

                e.Warehouse_sub_type_name = ((EWarehouseSubType)e.Warehouse_sub_type).GetDescription();
                e.Warehouse_type_name = ((EWarehouseType)e.Warehouse_type).GetDescription();
                e.Warehouse_effect_status_name = ((EWarehouseEffectStatus)e.Warehouse_effect_status).GetDescription();
            });

            return result;
        }

    }
}
