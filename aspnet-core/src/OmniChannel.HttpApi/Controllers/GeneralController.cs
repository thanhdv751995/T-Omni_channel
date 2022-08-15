using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.DeleteProducts;
using OmniChannel.DeleteProductsc;
using OmniChannel.General.BackgroundJob.Shopee;
using OmniChannel.General.GProducts;
using OmniChannel.General.Images;
using OmniChannel.General.LinkedProducts;
using OmniChannel.Images;
using OmniChannel.Products;
using OmniChannel.Shopee.Images;
using OmniChannel.Shopee.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace OmniChannel.Controllers
{
    [Route("api/g-product")]
    public class GeneralController : OmniChannelController
    {
        private readonly ProductAppService _productAppService;
        private readonly GProductAppService _gProductAppService;
        private readonly ProductBackgroundJobSPAppService _productBackgroundJobSPAppService;
        private readonly ProductSPAppService _productSPAppService;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly ImageSPAppService _imageSPAppService;
        private readonly ImageAppService _imageAppService;

        public GeneralController(ProductAppService productAppService,
            GProductAppService gProductAppService,
            ProductBackgroundJobSPAppService productBackgroundJobSPAppService,
            ProductSPAppService productSPAppService,
            ChannelAuthenticationAppService channelAuthenticationAppService,
            ImageSPAppService imageSPAppService,
            ImageAppService imageAppService)
        {
            _productAppService = productAppService;
            _gProductAppService = gProductAppService;
            _productBackgroundJobSPAppService = productBackgroundJobSPAppService;
            _productSPAppService = productSPAppService;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _imageSPAppService = imageSPAppService;
            _imageAppService = imageAppService;
        }

        [HttpPost("upload-image")]
        public async Task<ResponseGUploadImageDto> UploadImage(IFormFile file)
        {
            ResponseGUploadImageDto responseGUploadImageDto = new();
            
            Request.Headers.Add("ChannelAuthentication", "aed64a45b56ae0e0797283e057303c664825dc6caef82764a7b6078417e2fd4e");

            var channel_token = Request.Headers["ChannelAuthentication"];

            var e_channel = await _channelAuthenticationAppService.GetEChannel(channel_token);

            switch (e_channel)
            {
                case EChannel.TiktokShop:
                    {

                        break;
                    }
                case EChannel.Shopee:
                    {
                        responseGUploadImageDto = await _imageSPAppService.UploadImage(file);
                        break;
                    }
            }

            return responseGUploadImageDto;
        }

        [HttpGet("products")]
        public async Task<PagedResultDto<ListGProductDto>> Products(EChannel e_channel,
            [Required] string app,
            [Required] string client_id,
            [Required] int skip,
            [Required] int take,
            string shopId,
            bool? is_Linked) =>
            await _gProductAppService.Products(e_channel, app, client_id, skip, take, shopId, is_Linked);

        [HttpGet("get-list-products")]
        public async Task<PagedResultDto<ListGProductDto>> GetListProducts(EChannel e_channel,
          [Required] string app,
          [Required] string client_id,
          [Required] int skip,
          [Required] int take,
          string shopId,
          bool? is_Linked) =>
          await _gProductAppService.GetListProducts(e_channel, app, client_id, skip, take, shopId, is_Linked);


        [HttpPost("create")]
        public void CreateGProduct([FromBody] CreateGProductDto createGProductDto)
        {
            var channel_token = Request.Headers["ChannelAuthentication"];

            var convertToCreateGProductDto = _gProductAppService.CreateGProduct(createGProductDto);

            switch (createGProductDto.E_channel)
            {
                case EChannel.TiktokShop:
                    {
                        _productAppService.CreateGProductBackgroundJob(convertToCreateGProductDto, channel_token);

                        break;
                    }
                case EChannel.Shopee:
                    {
                        _productBackgroundJobSPAppService.CreateGProductSPBackgroundJob(convertToCreateGProductDto, channel_token);

                        break;
                    }
            }
        }

        [HttpPost("creates")]
        public void CreateGProductByList([FromBody] List<CreateGProductDto> listCreateGProductDto)
        {
            foreach(var createGProductDto in listCreateGProductDto)
            {
                CreateGProduct(createGProductDto);
            }
        }

        [HttpPut("update")]
        public void UpdateGProduct([FromBody] CreateGProductDto createGProductDto)
        {

            var channel_token = Request.Headers["ChannelAuthentication"];

            var convertToUpdateGProductDto = _gProductAppService.UpdateGProduct(createGProductDto);

            switch (createGProductDto.E_channel)
            {
                case EChannel.TiktokShop:
                    {
                        _productAppService.UpdateGProductBackgroundJob(convertToUpdateGProductDto, channel_token);
                        break;
                    }
                case EChannel.Shopee:
                    {
                        _productBackgroundJobSPAppService.UpdateGProductSPBackgroundJob(convertToUpdateGProductDto, channel_token);

                        break;
                    }
            }
        }

        [HttpPut("updates")]
        public void UpdateGProductByList([FromBody] List<CreateGProductDto> listCreateGProductDto)
        {
            foreach (var createGProductDto in listCreateGProductDto)
            {
                UpdateGProduct(createGProductDto);
            }
        }

        [HttpDelete("products")]
        public void DeleteProduct([FromBody] DeleteGProductDto deleteGProductDto)
        {

            var channel_token = Request.Headers["ChannelAuthentication"];

             _gProductAppService.DeleteProductAsync(deleteGProductDto, channel_token);
        }

        /// <summary>
        /// <p>e_channel:</p>
        /// <p>0: TiktokShop</p>
        /// <p>1: Lazada</p>
        /// <p>2: Tiki</p>
        /// <p>3: Shopee</p>
        /// </summary>
        /// <param name="e_channel"></param>
        /// <param name="client_product_id"></param>
        /// <param name="listSku"></param>
        /// <returns></returns>
        [HttpPut("price")]
        public void UpdatePrice(EChannel e_channel, string client_product_id, [FromBody] List<GUpdatePriceDto> listSku)
        {
            var channel_token = Request.Headers["ChannelAuthentication"];

            switch (e_channel)
            {
                case EChannel.TiktokShop:
                    {
                        _productAppService.UpdateGProductPriceBackgroundJob(e_channel, client_product_id, channel_token, listSku);

                        break;
                    }
                case EChannel.Shopee:
                    {
                        _productBackgroundJobSPAppService.UpdateGProductPriceSPBackgroundJob(channel_token, e_channel, client_product_id, listSku);

                        break;
                    }
            }
        }

        [HttpPut("link-addlink")]
        public async Task AddLinkedProduct([FromBody] List<AddLinkedProductDto> addLinkedProductDtos)
        {
            await _gProductAppService.AddLinkedProduct(addLinkedProductDtos);
        }

        [HttpPost("active")]
        public async Task Active(EChannel e_channel, [FromBody] ActivateGProductDto activateGProductDto)
        {

            var channel_token = Request.Headers["ChannelAuthentication"];

            switch (e_channel)
            {
                case EChannel.TiktokShop:
                    {
                        await _productAppService.ActivateDeActiveGProduct(e_channel, channel_token, true, activateGProductDto);
                        break;
                    }
                case EChannel.Shopee:
                    {
                        await _productSPAppService.UnlistProduct(channel_token, e_channel, true, activateGProductDto);
                        break;
                    }
            }
        }

        [HttpPost("deactive")]
        public async Task DeActive(EChannel e_channel, [FromBody] ActivateGProductDto activateGProductDto)
        {

            var channel_token = Request.Headers["ChannelAuthentication"];

            switch (e_channel)
            {
                case EChannel.TiktokShop:
                    {
                        await _productAppService.ActivateDeActiveGProduct(e_channel, channel_token, false, activateGProductDto);
                        break;
                    }
                case EChannel.Shopee:
                    {
                        await _productSPAppService.UnlistProduct(channel_token, e_channel, false, activateGProductDto);
                        break;
                    }
            }
        }
        [HttpPost("ProductLinkedList")]
        public async Task<PagedResultDto<ProductLinkedListDto>> ProductLinkedList([FromBody]RequestSearchListProductDto searchListProductDto, string app, string clien_id ,
            string shop_id, bool? is_Linked)
        {
            
           return await _gProductAppService.ProductLinkedList(searchListProductDto, app, clien_id, shop_id , is_Linked);
        }

        [HttpPut("link-unlink")]
        public async Task UpdateListProductLinked(EChannel e_channel, [FromBody] UpdateProductLinkedDto updateProductLinkedDto)
        {
            var channel_token = Request.Headers["ChannelAuthentication"];

            await _gProductAppService.UpdateListProductLinked(e_channel, channel_token, updateProductLinkedDto);
        }

        [HttpPut("product_synchronization")]
        public async Task<string> ProductSynchronization([FromBody]List<SynchronizedProductDto> synchronizedProductDtos)
        {
            var result = await _gProductAppService.ProductSynchronization(synchronizedProductDtos);

            return result;
        }
    }
}
