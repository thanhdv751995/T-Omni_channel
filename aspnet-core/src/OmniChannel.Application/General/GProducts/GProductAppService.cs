using JetBrains.Annotations;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniChannel.BackgroundJob;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.Clients;
using OmniChannel.General.LinkedProducts;
using OmniChannel.Products;
using OmniChannel.ProductStatuss;
using OmniChannel.Shares;
using OmniChannel.Shopee.Products;
using OmniChannel.TiktokShop.CreateProducts;
using OmniChannel.TiktokShop.ProductImages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.General.GProducts
{
    [RemoteService(false)]
    public class GProductAppService : OmniChannelAppService
    {
        private readonly string TiktokShopDefaultImageId = OmniChannelConsts.TiktokShopDefaultImageId;
        private readonly string TiktokShopDefaultPackageWeight = OmniChannelConsts.TiktokShopDefaultPackageWeight;
        private readonly string ERROR_NOTNULL = OmniChannelErrorConsts.NOTNULL;
        private readonly string DESCRIPTION_DEFAULT_VALUE = OmniChannelErrorConsts.DESCRIPTION_DEFAULT_VALUE;

        private readonly ShareAppService _shareAppService;
        private readonly IProductRepository _productRepository;
        private readonly BackgroundJobAppService _backgroundJobAppService;
        private readonly ProductAppService _productAppService;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;

        public GProductAppService(ShareAppService shareAppService, IProductRepository productRepository,
            ProductAppService productAppService, IProductImageRepository productImageRepository,
            BackgroundJobAppService backgroundJobAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository)
        {
            _shareAppService = shareAppService;
            _productRepository = productRepository;
            _productAppService = productAppService;
            _productImageRepository = productImageRepository;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _backgroundJobAppService = backgroundJobAppService;
        }

        public CreateGProductDto CreateGProduct(CreateGProductDto createGProductDto)
        {
            switch (createGProductDto.E_channel)
            {
                case EChannel.TiktokShop:
                    createGProductDto = ConvertToProductTiktokShopDto(createGProductDto);

                    break;
                case EChannel.Shopee:
                    createGProductDto = ConvertToProductShopeeDto(createGProductDto);

                    break;
            }

            return createGProductDto;
        }

        public CreateGProductDto UpdateGProduct(CreateGProductDto createGProductDto)
        {
            switch (createGProductDto.E_channel)
            {
                case EChannel.TiktokShop:
                    createGProductDto = ConvertToProductTiktokShopDto(createGProductDto);

                    break;
                case EChannel.Shopee:
                    createGProductDto = ConvertToProductShopeeDto(createGProductDto);

                    break;
            }

            return createGProductDto;
        }

        #region Convert to TiktokShop

        private CreateGProductDto ConvertToProductTiktokShopDto(CreateGProductDto createGProductDto)
        {
            if (createGProductDto.Client_product_id.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"{nameof(createGProductDto.Client_product_id)} {ERROR_NOTNULL}");
            }

            if (createGProductDto.Client_category_id.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"{nameof(createGProductDto.Client_category_id)} {ERROR_NOTNULL}");
            }

            if (createGProductDto.Description.IsNullOrWhiteSpace())
            {
                createGProductDto.Description = $"<p>{createGProductDto.Description} {DESCRIPTION_DEFAULT_VALUE}</p>";
            }
            else
            {
                if (!createGProductDto.Description.StartsWith('<') && !createGProductDto.Description.EndsWith('>'))
                {
                    createGProductDto.Description = $"<p>{createGProductDto.Description}</p>";
                }
            }

            if (createGProductDto.Package_weight.IsNullOrWhiteSpace())
            {
                createGProductDto.Package_weight = TiktokShopDefaultPackageWeight;
            }

            if (createGProductDto.Image_ids.IsNullOrEmpty() || !createGProductDto.Image_ids.Any())
            {
                createGProductDto.Image_ids = new List<string>(new string[] { TiktokShopDefaultImageId });
            }

            if (createGProductDto.Available_stock == null || createGProductDto.Available_stock < 0)
            {
                createGProductDto.Available_stock = 0;
            }

            return createGProductDto;
        }

        #endregion

        #region Convert to Shopee

        private CreateGProductDto ConvertToProductShopeeDto(CreateGProductDto createGProductDto)
        {
            if (createGProductDto.Client_product_id.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"{nameof(createGProductDto.Client_product_id)} {ERROR_NOTNULL}");
            }

            if (createGProductDto.Client_category_id.IsNullOrEmpty())
            {
                throw new UserFriendlyException($"{nameof(createGProductDto.Client_category_id)} {ERROR_NOTNULL}");
            }

            if (createGProductDto.Description.IsNullOrWhiteSpace())
            {
                createGProductDto.Description = $"{createGProductDto.Description} {DESCRIPTION_DEFAULT_VALUE}";
            }

            if (createGProductDto.Package_weight.IsNullOrWhiteSpace())
            {
                createGProductDto.Package_weight = TiktokShopDefaultPackageWeight;
            }

            if (createGProductDto.Available_stock == null || createGProductDto.Available_stock < 0)
            {
                createGProductDto.Available_stock = 0;
            }

            return createGProductDto;
        }

        #endregion

        public async Task<PagedResultDto<ListGProductDto>> Products(EChannel eChannel,string app, string client_id, int skip, int take , string shopId, bool? is_Linked)
        {  
            var channel =  _channelAuthenticationRepository.GetListAsync(x=>x.App == app && x.Client_id == client_id).Result.Select(x=>x.Shop_id).ToList();

            var listProductPaged = await _productRepository.GetListPagedAsync(channel , eChannel, skip ,take, shopId , is_Linked);

            List<ListGProductDto> listGProductDto = new();

            foreach (var product in listProductPaged.Item2)
            {
                var productChannelDataDto = _shareAppService.ConvertStringToDto<DataChannelProductDto>(product.Channel_Data);

                ListGProductDto gProductDto = new();

                List<GProductImageDto> gProductImageDtos = new();

                GProductImageDto gProductImageDto = new();

                gProductDto.Client_product_id = product.Client_Product_Id;

                gProductDto.E_channel_name = ShareAppService.GetEnumName<EChannel>(gProductDto.E_channel);

                gProductDto.Shop_id = product.Shop_Id;

                gProductDto.Shop_name = _channelAuthenticationRepository.SingleOrDefaultAsync(x => x.Shop_id == product.Shop_Id).Result.Seller_name;

                gProductDto.Product_name = productChannelDataDto.Product_name;

                gProductDto.Channel_product_id = product.Channel_Product_Id;

                gProductDto.Is_linked = product.IsLinked;

                gProductDto.Last_connection_time = product.CreationTime;

                gProductDto.Product_status = (EProductStatus)productChannelDataDto.Product_status;

                gProductDto.Product_status_name = ShareAppService.GetEnumName<EProductStatus>(productChannelDataDto.Product_status);

                //description bỏ các html trong đoạn string
                gProductDto.Description = Regex.Replace(productChannelDataDto.Description, @"<(.|\n)*?>", string.Empty);

                gProductDto.Category_id = product.Client_Category_Id;
                gProductDto.Client_category_id = product.Client_Category_Id;

                //lấy url từ database khi có id
                if(productChannelDataDto.Images != null)
                {
                    foreach (var img in productChannelDataDto.Images)
                    {
                        gProductImageDto.Id = img.id;
                        if(await _productImageRepository.AnyAsync(x => x.Img_id == img.id))
                        {
                            gProductImageDto.Url = _productImageRepository.FirstOrDefaultAsync(x => x.Img_id == img.id)?.Result.Img_url;
                        }
                        else
                        {
                            gProductImageDto.Url = null;
                        }    
                        gProductImageDtos.Add(gProductImageDto);
                    }

                    gProductDto.Images = gProductImageDtos;
                }    
                gProductDto.Skus = productChannelDataDto.Skus;

                listGProductDto.Add(gProductDto);
            }

            return new PagedResultDto<ListGProductDto>(
                listProductPaged.Item1 , listGProductDto
                );
        }

        /// <summary>
        /// lấy tất cả sản phẩm ở database
        /// </summary>
        /// <param name="eChannel"> chọn kênh bán hàng</param>
        /// <param name="app"> tpos</param>
        /// <param name="client_id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="shopId"></param>
        /// <param name="is_Linked"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ListGProductDto>> GetListProducts(EChannel eChannel, string app, string client_id, int skip, int take, string shopId, bool? is_Linked)
        {
            var channel = _channelAuthenticationRepository.GetListAsync(x => x.App == app && x.Client_id == client_id).Result.Select(x => x.Shop_id).ToList();

            var listProductPaged = await _productRepository.GetListPagedAsync(channel, eChannel, skip, take, shopId, is_Linked);

            List<ListGProductDto> listGProductDto = new();

            foreach (var product in listProductPaged.Item2)
            {
                if(eChannel == EChannel.TiktokShop)
                {
                    var productChannelDataDto = _shareAppService.ConvertStringToDto<DataChannelProductDto>(product.Channel_Data);

                    ListGProductDto gProductDto = new();

                    List<GProductImageDto> gProductImageDtos = new();

                    GProductImageDto gProductImageDto = new();

                    gProductDto.Client_product_id = product.Client_Product_Id;

                    gProductDto.E_channel_name = ShareAppService.GetEnumName<EChannel>(gProductDto.E_channel);

                    gProductDto.Shop_id = product.Shop_Id;

                    gProductDto.Shop_name = _channelAuthenticationRepository.SingleOrDefaultAsync(x => x.Shop_id == product.Shop_Id).Result.Seller_name;

                    gProductDto.Product_name = productChannelDataDto.Product_name;

                    gProductDto.Channel_product_id = product.Channel_Product_Id;

                    gProductDto.Is_linked = product.IsLinked;

                    gProductDto.Last_connection_time = product.CreationTime;

                    gProductDto.Product_status = (EProductStatus)productChannelDataDto.Product_status;

                    gProductDto.Product_status_name = ShareAppService.GetEnumName<EProductStatus>(productChannelDataDto.Product_status);

                    //description bỏ các html trong đoạn string
                    gProductDto.Description = Regex.Replace(productChannelDataDto.Description, @"<(.|\n)*?>", string.Empty);

                    gProductDto.Category_id = product.Client_Category_Id;
                    gProductDto.Client_category_id = product.Client_Category_Id;

                    //lấy url từ database khi có id
                    if (productChannelDataDto.Images != null)
                    {
                        foreach (var img in productChannelDataDto.Images)
                        {
                            gProductImageDto.Id = img.id;
                            if (await _productImageRepository.AnyAsync(x => x.Img_id == img.id))
                            {
                                gProductImageDto.Url = _productImageRepository.FirstOrDefaultAsync(x => x.Img_id == img.id)?.Result.Img_url;
                            }
                            else
                            {
                                gProductImageDto.Url = null;
                            }
                            gProductImageDtos.Add(gProductImageDto);
                        }

                        gProductDto.Images = gProductImageDtos;
                    }
                    gProductDto.Skus = productChannelDataDto.Skus;

                    listGProductDto.Add(gProductDto);
                }
              if(eChannel == EChannel.Shopee)
                {
                    var productChannelDataDto = _shareAppService.ConvertStringToDto<ProductBaseInfoDto>(product.Channel_Data);

                    ListGProductDto gProductDto = new();

                    List<GProductImageDto> gProductImageDtos = new();

                    GProductImageDto gProductImageDto = new();

                    gProductDto.Client_product_id = product.Client_Product_Id;

                    gProductDto.E_channel_name = ShareAppService.GetEnumName<EChannel>(gProductDto.E_channel);

                    gProductDto.Shop_id = product.Shop_Id;

                    gProductDto.Shop_name = _channelAuthenticationRepository.SingleOrDefaultAsync(x => x.Shop_id == product.Shop_Id).Result.Seller_name;

                    gProductDto.Product_name = productChannelDataDto.item_name;

                    gProductDto.Channel_product_id = product.Channel_Product_Id;

                    gProductDto.Is_linked = product.IsLinked;

                    gProductDto.Last_connection_time = product.CreationTime;

                    gProductDto.Product_status_name = productChannelDataDto.item_status;

                    gProductDto.Description = productChannelDataDto.description;

                    gProductDto.Category_id = product.Client_Category_Id;

                    gProductDto.Client_category_id = product.Client_Category_Id;

                    //lấy url từ database khi có id
                    if (productChannelDataDto.image != null)
                    {
                        foreach (var img in productChannelDataDto.image.image_id_list)
                        {
                            gProductImageDto.Id = img;
                            if (await _productImageRepository.AnyAsync(x => x.Img_id == img))
                            {
                                gProductImageDto.Url = _productImageRepository.FirstOrDefaultAsync(x => x.Img_id == img)?.Result.Img_url;
                            }
                            else
                            {
                                gProductImageDto.Url = null;
                            }
                            gProductImageDtos.Add(gProductImageDto);
                        }

                        gProductDto.Images = gProductImageDtos;
                    }
                 //   gProductDto.Skus = productChannelDataDto.Skus;

                    listGProductDto.Add(gProductDto);
                }
            }

            return new PagedResultDto<ListGProductDto>(
                listProductPaged.Item1, listGProductDto
                );
        }

        //delete product
        public void DeleteProductAsync(DeleteGProductDto deleteGProductDto, string Channel_token)
        {
            switch (deleteGProductDto.E_channel)
            {
                case EChannel.TiktokShop:
                    _productAppService.DeleteGProductBackgroundJob(deleteGProductDto.Client_product_ids, Channel_token);
                    break;
            }
        }
        /// <summary>
        /// Lấy sản phẩm danh sách đã liên kết
        /// </summary>
        /// <param name="searchListProductDto"></param>
        /// <param name="app"></param>
        /// <param name="clien_id"></param>
        /// <param name="shopId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductLinkedListDto>> ProductLinkedList(RequestSearchListProductDto searchListProductDto, string app, string clien_id,
            string shopId, bool? isLinked)
        {
            if (shopId.IsNullOrWhiteSpace())
            {
                shopId = "";
            }
            List<ProductLinkedListDto> listResponseDataProductTikTokShop = new();

            //Lấy danh sách channel từ app và client_id

            var channel_authen = await _channelAuthenticationRepository.GetListAsync(x => x.App == app && x.Client_id == clien_id);

            //get danh sách product bằng shop_id từ chennal-authen mới lấy được

            foreach (var authen in channel_authen)
            {
                var responseDataProductTikTokShop = await _productAppService.Products(searchListProductDto, authen.Channel_token);

                var listProductMap = ObjectMapper.Map<List<DetailProductListDto>, List<ProductLinkedListDto>>(responseDataProductTikTokShop.Data.Products);

                foreach (var productMap in listProductMap)
                {
                    productMap.Is_linked = await _productRepository.AnyAsync(x => x.Channel_Product_Id == productMap.Id && x.Client_Product_Id != "");

                    productMap.Shop_id = authen.Shop_id;

                    productMap.Shop_name = authen.Seller_name;
                }

                listResponseDataProductTikTokShop.AddRange(listProductMap);

                _backgroundJobAppService.CheckChangeProductTiktokShop();
            }

            var result = listResponseDataProductTikTokShop.Skip(searchListProductDto.page_number - 1).Take(searchListProductDto.page_size).Where(x => !x.Shop_id.Contains(shopId) || isLinked == null || x.Is_linked == isLinked).ToList();

            var totalCount = listResponseDataProductTikTokShop.Count(x => !x.Shop_id.Contains(shopId) || isLinked == null || x.Is_linked == isLinked);
            return new PagedResultDto<ProductLinkedListDto>(
                totalCount,
                result
            );
        }
        /// <summary>
        /// Thêm liên kết sản phẩm
        /// </summary>
        /// <returns></returns>
        public async Task AddLinkedProduct(List<AddLinkedProductDto> addLinkedProductDtos)
        {
            try
            {
                foreach (var addLinkedProductDto in addLinkedProductDtos)
                {
                    var product = await _productRepository.GetAsync(x => x.Channel_Product_Id == addLinkedProductDto.Channel_product_id);

                    product.Client_Product_Id = addLinkedProductDto.Client_product_id;
                    product.IsLinked = true;
                    product.Client_Data = addLinkedProductDto.Client_data;

                    await _productRepository.UpdateAsync(product);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Something wrong " + ex.Message);
            }
        }


        /// <summary>
        /// thêm , hủy liên kết sản phẩm
        /// </summary>
        /// <param name="e_channel"></param>
        /// <param name="channel_token"></param>
        /// <param name="updateProductLinkedDto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [RemoteService(false)]
        public async Task UpdateListProductLinked(EChannel e_channel, string channel_token, UpdateProductLinkedDto updateProductLinkedDto)
        {
            if (await _channelAuthenticationRepository.AnyAsync(x => x.Channel_token == channel_token))
            {
                List<Product> listProduct = new();

                foreach (var client_product_id in updateProductLinkedDto.Client_product_ids)
                {
                    var product = await _productRepository.GetAsync(x => x.E_Channel == e_channel && x.Client_Product_Id == client_product_id);

                    product.IsLinked = !product.IsLinked;

                    listProduct.Add(product);
                }

                await _productRepository.UpdateManyAsync(listProduct);
            }
            else
                throw new UserFriendlyException($"{ERROR_NOTNULL} {nameof(channel_token)}");
        }

        public async Task<string> ProductSynchronization(List<SynchronizedProductDto> synchronizedProductDtos)
        {
            string result = "";

            foreach (var product in synchronizedProductDtos)
            {             
                var channel_product = await _productRepository.GetAsync(x => x.Channel_Product_Id == product.Channel_product_id);

                channel_product.IsLinked = true;

                var clientData = JsonConvert.DeserializeObject<ClientDataDto>(product.Client_data.ToString()); ;

                var productSkus = _shareAppService.ConvertStringToDto<DataChannelProductDto>(channel_product.Channel_Data);

                if(productSkus.Skus.Count != clientData.Variants.Count)
                {
                    throw new BusinessException("Số lượng biến thể không đồng bộ");
                }
                else
                {
                    foreach(var client_product in clientData.Variants)
                    {
                        if(Int32.Parse(client_product.Price) == 0)
                        {
                            throw new BusinessException("Không thể đồng bộ sản phẩm có giá bằng 0");
                        }    
                        foreach(var chanelAttribute in productSkus.Skus)
                        {
                            chanelAttribute.Original_price = client_product.Price;

                            chanelAttribute.Stock_infos[0].Available_stock = clientData.Uom.Factor;

                            result = ShareAppService.CheckVariantProduct(client_product.AttributeValues, chanelAttribute.Sales_attributes);

                            channel_product.Channel_Data = ShareAppService.ConvertDtoToString(productSkus);
                        }
                    }
                    channel_product.Client_Data = ShareAppService.ConvertDtoToString(clientData);
                }
                //xử lý update sản phẩm của tpos lên tiktok-shop

                await _productRepository.UpdateAsync(channel_product);

                //CreateGProductDto createGProductDto = ObjectMapper.Map<DataChannelProductDto, CreateGProductDto>
                //    (_shareAppService.ConvertStringToDto<DataChannelProductDto>(channel_product.Channel_Data));

                //createGProductDto.E_channel = channel_product.E_Channel;
                //createGProductDto.Client_product_id = clientData.Id;
                //createGProductDto.Client_category_id = clientData.Category.Id;

                // _gProductAppService.UpdateGProduct(createGProductDto);
            }
            return result;
        }

        public async Task GUpdatePriceAll(GUpdatePriceAllDto gUpdatePriceAllDto)
        {
            var listProduct = await _productRepository.GetListAsync();
            listProduct.ForEach(x =>
            {
                var channelData = _shareAppService.ConvertStringToDto<DataChannelProductDto>(x.Channel_Data);

                channelData.Skus.ForEach(sku =>
                {   
                    if (gUpdatePriceAllDto.Percent_increase != 0)
                    {
                        sku.Original_price = (long.Parse(sku.Original_price) + (long.Parse(sku.Original_price) * gUpdatePriceAllDto.Percent_increase / 100)).ToString();
                    }
                    if(gUpdatePriceAllDto.Percent_decrease != 0)
                    {
                        sku.Original_price = (long.Parse(sku.Original_price) - (long.Parse(sku.Original_price) * gUpdatePriceAllDto.Percent_increase / 100)).ToString();
                    }
                    if (gUpdatePriceAllDto.Same_price != 0)
                    {
                        sku.Original_price = gUpdatePriceAllDto.Same_price.ToString();
                    }
                });

            });
   
        }
    }
}
