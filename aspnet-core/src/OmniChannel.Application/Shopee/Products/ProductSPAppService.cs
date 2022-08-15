using Microsoft.AspNetCore.Mvc;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.General.GProducts;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Products;
using OmniChannel.ProductStatuss;
using OmniChannel.Shares;
using OmniChannel.Shopee.Prices;
using OmniChannel.Shopee.Products.CreateProductSP;
using OmniChannel.Shopee.Products.DeleteProductSP;
using OmniChannel.Shopee.Products.UpdateProductSP;
using OmniChannel.Shopee.ResponseData;
using OmniChannel.TiktokShop.CreateProducts;
using OmniChannel.TiktokShop.ProductImages;
using OmniChannel.UpdateProducts;
using OmniChannel.CreateProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.Shopee.Products
{
    public class ProductSPAppService : OmniChannelAppService, IProductSPAppService
    {
        private readonly string API_GET_PRODUCT_LIST = ShopeeConst.API_GET_PRODUCT_LIST;
        private readonly string API_GET_PRODUCT_BASE_INFO = ShopeeConst.API_GET_PRODUCT_BASE_INFO;
        private readonly string API_ADD_ITEM = ShopeeConst.API_ADD_ITEM;
        private readonly string API_DELETE_ITEM = ShopeeConst.API_DELETE_ITEM;
        private readonly string API_UPDATE_ITEM = ShopeeConst.API_UPDATE_ITEM;
        private readonly string API_UPDATE_PRICE = ShopeeConst.API_UPDATE_PRICE;
        private readonly string API_UNLIST_ITEM = ShopeeConst.API_UNLIST_ITEM;

        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly List<string> item_status_default = new(new string[] { "NORMAL" });
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly ShareAppService _shareAppService;
        private readonly IProductRepository _productRepository;
        private readonly ProductAppService _productAppService;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ProductImageManager _productImageManager;
        private readonly ProductManager _productManager;

        public ProductSPAppService(ShareAppService shareAppService,
            ChannelAuthenticationAppService channelAuthenticationAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository,
            ProductAppService productAppService,
            IProductRepository productRepository,
            ProductManager productManager,
            IProductImageRepository productImageRepository,
            ProductImageManager productImageManager)
        {
            _shareAppService = shareAppService;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _productRepository = productRepository;
            _productAppService = productAppService;
            _productImageRepository = productImageRepository;
            _productImageManager = productImageManager;
            _productManager = productManager;
        }

        //item_status: NORMAL/BANNED/DELETED/UNLIST
        public async Task<ResponseDataSPDto<ResponseListProductBaseInfoDto>> GetProducts(long shop_id, 
            string access_token, 
            List<string> item_status, 
            int offset = 0, 
            int page_size = 10)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"offset={offset}", $"page_size={page_size}");

            if (!item_status.Any())
            {
                item_status = item_status_default;
            }

            foreach (var item in item_status)
            {
                requestParameters.Add($"&item_status={item}");
            }

            var url = _shareAppService.GetShopeeUrl(API_GET_PRODUCT_LIST, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            var listProductSP = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseProductListDto>>(httpResponseMessage);

            return await GetBaseInfo(shop_id, access_token, listProductSP.response.item.Select(x => x.item_id).ToList());
        }

        public async Task<ResponseDataSPDto<ResponseListProductBaseInfoDto>> GetBaseInfo(long shop_id, 
            string access_token, 
            List<long> item_id_list)
        {
            var requestParameters = ShareAppService.GetRequestParametersShopee($"item_id_list={string.Join(",", item_id_list)}");

            var url = _shareAppService.GetShopeeUrl(API_GET_PRODUCT_BASE_INFO, shop_id, access_token, requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseListProductBaseInfoDto>>(httpResponseMessage);
        }

        public async Task<ResponseDataSPDto<ResponseCreateProductSPDto>> CreateProduct(long shop_id, 
            string access_token, 
            [FromBody] RequestCreateProductSPDto createProductSPDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_ADD_ITEM, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, createProductSPDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseCreateProductSPDto>>(httpResponseMessage);
        }

        [HttpPost]
        public async Task<string> Delete(long shop_id, 
            string access_token, 
            [FromBody] DeleteProductSPDto deleteProductSPDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_DELETE_ITEM, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, deleteProductSPDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        [HttpPost]
        public async Task<ResponseDataSPDto<ResponseCreateProductSPDto>> Update(long shop_id, 
            string access_token, 
            [FromBody] RequestUpdateProductSPDto requestUpdateProductSPDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_UPDATE_ITEM, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestUpdateProductSPDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseCreateProductSPDto>>(httpResponseMessage);
        }

        [HttpPost]
        public async Task<ResponseDataSPDto<ResponseCreateProductSPDto>> UpdatePrice(long shop_id, 
            string access_token, 
            [FromBody] RequestUpdatePriceDto requestUpdatePriceDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_UPDATE_PRICE, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestUpdatePriceDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataSPDto<ResponseCreateProductSPDto>>(httpResponseMessage);
        }


        #region unlist

        [HttpPost]
        public async Task<string> Unlist(long shop_id,
            string access_token,
            [FromBody] RequestUnlistItemDto requestUnlistItemDto)
        {
            var url = _shareAppService.GetShopeeUrl(API_UNLIST_ITEM, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestUnlistItemDto);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task UnlistProduct(string channel_token, EChannel e_channel, bool is_unlist, ActivateGProductDto activateGProductDto)
        {
            var channel_authentication = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token
            && x.E_Channel == e_channel);

            List<UnlistItemDto> item_list = new();
            List<Product> listProduct = new(); 

            foreach(var client_product_id in activateGProductDto.client_product_ids)
            {
                var product = await _productRepository.GetAsync(x => x.Client_Product_Id == client_product_id && x.E_Channel == e_channel);
                product.IsActive = is_unlist;

                item_list.Add(new UnlistItemDto()
                {
                    item_id = long.Parse(product.Channel_Product_Id),
                    unlist = !is_unlist
                });

                listProduct.Add(product);
            }

            RequestUnlistItemDto requestUnlistItemDto = new()
            {
                item_list = item_list
            };

            await Unlist(long.Parse(channel_authentication.Shop_id), channel_authentication.Access_token, requestUnlistItemDto);

            await _productRepository.UpdateManyAsync(listProduct);
        }

        #endregion

        public async Task<List<ProductBaseInfoDto>> GetListProductByChannelToken(string channelToken)
        {
            var channelTK = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channelToken);

            var listProducts = await _productRepository.GetListAsync(x => x.Shop_Id == channelTK.Shop_id);
            List< ProductBaseInfoDto> listProductBaseInfo = new List< ProductBaseInfoDto>();
            foreach (var product in listProducts)
            {
                 var productChannelDataDto = _shareAppService.ConvertStringToDto<ProductBaseInfoDto>(product.Channel_Data);
                ProductBaseInfoDto productBaseInfoDto = new ProductBaseInfoDto();
                productBaseInfoDto.item_name = productChannelDataDto.item_name;

                listProductBaseInfo.Add(productChannelDataDto);
            }

            return listProductBaseInfo;
        }

        public async Task AutomaticProductUpdate()
        {
            var listChannelAuthen = await _channelAuthenticationAppService.GetListChannelShopee();

            if(listChannelAuthen != null)
            {
                foreach(var channelAuthen in listChannelAuthen)
                {
                    List<string> itemStatus = new List<string>();
                    var listProductSP = await GetProducts(long.Parse(channelAuthen.Shop_id),channelAuthen.Access_token, itemStatus);
                    foreach (var product in listProductSP.response.item_list)
                    {
                        var productDB = await _productRepository.FindAsync(x => x.Channel_Product_Id == product.item_id.ToString());
                        List<long> items = new List<long>();
                        items.Add(product.item_id);
                        var productDetail = await GetBaseInfo(long.Parse(channelAuthen.Shop_id), channelAuthen.Access_token, items);// product detail

                        List<CreateSkusProductDto> skusProductsDTo = new();

                        List<SalesAttributesDto> listSaleAtributeDto = new();

                        CreateSkusProductDto skusDto = new()
                        {
                            Id = product.item_sku,
                            Sales_attributes = listSaleAtributeDto,
                        };
                        if (product.attribute_list != null)
                        {
                            foreach (var atribute in product.attribute_list)
                            {
                                SalesAttributesDto saleAtributeDto = new()
                                {
                                    Id = GuidGenerator.Create(),
                                    Attribute_id = atribute.attribute_id.ToString(),
                                    Custom_value = atribute.attribute_value_list[0].original_value_name
                                };
                                listSaleAtributeDto.Add(saleAtributeDto);
                            }
                        }

                        skusProductsDTo.Add(skusDto);

                        //image
                        ImageProductInfoBaseDto images = new();

                        if (productDetail.response.item_list[0].image != null)
                        {
                            foreach (var image in productDetail.response.item_list[0].image.image_id_list)
                            {
                                var isAnyImage = await _productImageRepository.AnyAsync(x => x.Img_id == image);

                                var imageDetailProductId = image;

                                if (!isAnyImage)
                                {
                                    var imag = _productImageManager.CreateAsync(imageDetailProductId, image, 1);
                                    await _productImageRepository.InsertAsync(imag);
                                }
                                else
                                {
                                    var imag = await _productImageRepository.GetAsync(x => x.Img_id == imageDetailProductId);

                                    imag.Img_url = productDetail.response.item_list[0].image.image_url_list[0];
                                    await _productImageRepository.UpdateAsync(imag);
                                }



                                images.image_id_list = productDetail.response.item_list[0].image.image_id_list;
                                images.image_url_list = productDetail.response.item_list[0].image.image_url_list;

                            }
                            
                        }

                        if (productDB != null)
                        {
                            var productUD = await _productRepository.GetAsync(x => x.Channel_Product_Id == product.item_id.ToString());

                            ResponseUpdateProductSPDto updateProductDto = new()
                            {
                                item_id = productDetail.response.item_list[0].item_id,
                                category_id = productDetail.response.item_list[0].category_id,
                                item_status = productDetail.response.item_list[0].item_status,
                                item_name = productDetail.response.item_list[0].item_name,
                                description = productDetail.response.item_list[0].description,
                                images = images,
                                weight = float.Parse(productDetail.response.item_list[0].weight),
                                pre_order = productDetail.response.item_list[0].pre_order,
                                condition = productDetail.response.item_list[0].condition,
                                brand = productDetail.response.item_list[0].brand,
                                description_type = productDetail.response.item_list[0].description_type,
                                logistic_info = productDetail.response.item_list[0].logistic_info,

                            };
                            productUD.Channel_Data = ShareAppService.ConvertDtoToString(updateProductDto);

                             await _productRepository.UpdateAsync(productUD);
                        }
                        else
                        {
                            ResponseUpdateProductSPDto dataChannel = new()
                            {
                                item_id = productDetail.response.item_list[0].item_id,
                                category_id = productDetail.response.item_list[0].category_id,
                                item_status = productDetail.response.item_list[0].item_status,
                                item_name = productDetail.response.item_list[0].item_name,
                                description = productDetail.response.item_list[0].description,
                                images = images,
                                weight = float.Parse(productDetail.response.item_list[0].weight),
                                pre_order = productDetail.response.item_list[0].pre_order,
                                condition = productDetail.response.item_list[0].condition,
                                brand = productDetail.response.item_list[0].brand,
                                description_type = productDetail.response.item_list[0].description_type,
                                logistic_info = productDetail.response.item_list[0].logistic_info,

                            };
                            var dataCreate = ShareAppService.ConvertDtoToString(dataChannel);
                            string clientData = "";
                            var produ = _productManager.CreateAsync(channelAuthen.Shop_id, product.item_id.ToString(), "", "",Channels.EChannel.Shopee, dataCreate, clientData, true);
                            await _productRepository.InsertAsync(produ);
                        }
                    }
                }
            }
        }
    }
}
