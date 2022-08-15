using Microsoft.AspNetCore.Mvc;
using OmniChannel.ChannelAuthentications;
using OmniChannel.General.Authenticatios;
using OmniChannel.General.Shops;
using OmniChannel.Products;
using OmniChannel.TiktokShop.ChannelAuthentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace OmniChannel.Controllers
{
    /// <summary>
    /// cửa hàng
    /// </summary>
    [Route("api/linked-shop")]
    public class LinkedShopController : OmniChannelController
    {
        private readonly IShopAppService _shopService;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly ProductAppService _productAppService;


        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="orderService"></param>
        public LinkedShopController(IShopAppService shopService, ChannelAuthenticationAppService channelAuthenticationAppService, ProductAppService productAppService)
        {
            _shopService = shopService;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _productAppService = productAppService;
        }
        [HttpPost("create-linked-shop")]
        /// <summary>
        /// Thêm kết nối authentication
        /// </summary>
        /// <param name="gCreateAuthenticationDto"></param>
        /// <returns></returns>
        public async Task<ChennelAuthenMessage> CreateChannelAuthentication([FromBody]GCreateAuthenticationDto gCreateAuthenticationDto)
        {
            return await _channelAuthenticationAppService.CreateChannelAuthentication(gCreateAuthenticationDto);
        }
        /// <summary>
        /// Lấy danh sách shop
        /// </summary>
        /// <param name="app"></param>
        /// <param name="client_id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<GShopDto>> GetListShop(string app, string client_id)
        {
            return await _shopService.GetListShop(app, client_id);
        }
        /// <summary>
        /// thêm , hủy liên kết shop
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpPut("Update-linked")]
        public async Task UpdateLinkedShop(string channel_token)
        {
            await _shopService.UpdateLinkedShop(channel_token);
        }

        /// <summary>
        /// xóa shop 
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpDelete]      
        public async Task DeleteAsync(string channel_token)
        {
            await _channelAuthenticationAppService.DeleteAsync(channel_token);
        }

        /// <summary>
        /// Lấy chi tiêt gian hàng
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpGet("shop-linked-detail")]
        public async Task<ShopDetailDto> ShopDetail(string channel_token)
        {
            
            return await _shopService.ShopDetail(channel_token);
        }

        /// <summary>
        /// Đồng bộ sản phẩm theo gian hàng
        /// </summary>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        [HttpGet("Shop-Synchronization-Products")]
        public async Task ShopLinkedProducts(string channel_token)
        {
            await _productAppService.ShopLinkedProducts(channel_token);
        }

        /// <summary>
        /// Đồng bộ sản phẩm all
        /// </summary>
        /// <returns></returns>
        [HttpGet("Synchronization-Products-All")]
        public async Task GetListProductRepeat()
        {
            await _productAppService.GetListProductRepeat();
        }
    }
}
