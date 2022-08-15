using Hangfire;
using Microsoft.AspNetCore.Mvc;
using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.Domains.Shopee.Product;
using OmniChannel.General.BackGroundJobs.Shopee;
using OmniChannel.General.GProducts;
using OmniChannel.Products;
using OmniChannel.Shares;
using OmniChannel.Shopee.Logistics;
using OmniChannel.Shopee.Models;
using OmniChannel.Shopee.Prices;
using OmniChannel.Shopee.Products;
using OmniChannel.Shopee.Products.CreateProductSP;
using OmniChannel.Shopee.Products.UpdateProductSP;
using OmniChannel.Shopee.ResponseData;
using OmniChannel.Shopee.TierVariations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.General.BackgroundJob.Shopee
{
    public class ProductBackgroundJobSPAppService : OmniChannelAppService, IProductBackgroundJobSPAppService
    {
        private readonly string LOGISTICS_CHANNEL_NAME = ShopeeConst.LOGISTICS_CHANNEL_NAME;
        private readonly int MILLISECONDS_DELAY = 5000;

        private readonly ProductDomainSPAppService _productDomainSPAppService;
        private readonly ProductSPAppService _productSPAppService;
        private readonly LogisticsSPAppService _logisticsSPAppService;
        private readonly TierVariationAppService _tierVariationAppService;
        private readonly ShareAppService _shareAppService;
        private readonly ModelSPAppService _modelSPAppService;

        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public ProductBackgroundJobSPAppService(IBackgroundJobClient backgroundJobClient,
            ProductDomainSPAppService productDomainSPAppService,
            ProductSPAppService productSPAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository,
            ICategoryRepository categoryRepository,
            LogisticsSPAppService logisticsSPAppService,
            IProductRepository productRepository,
            TierVariationAppService tierVariationAppService,
            ShareAppService shareAppService,
            ModelSPAppService modelSPAppService)
        {
            _backgroundJobClient = backgroundJobClient;
            _productDomainSPAppService = productDomainSPAppService;
            _productSPAppService = productSPAppService;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _categoryRepository = categoryRepository;
            _logisticsSPAppService = logisticsSPAppService;
            _productRepository = productRepository;
            _tierVariationAppService = tierVariationAppService;
            _shareAppService = shareAppService;
            _modelSPAppService = modelSPAppService;
        }

        #region Tạo sản phẩm
        public async Task CreateGProduct(CreateGProductDto createGProductDto, string channel_token)
        {
            await HandleModelCreateAsync(createGProductDto);

            var channel_authentication = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token
            && x.E_Channel == createGProductDto.E_channel);

            var category_id = await GetChannelCategory(createGProductDto.Client_category_id, createGProductDto.E_channel);

            var logistics_info = await GetLogisticInfosAsync(channel_authentication);

            var image = GetImagesCreate(createGProductDto);

            RequestCreateProductSPDto createProductDto = new()
            {
                brand = new BrandCreateProductSPDto(),
                category_id = category_id,
                item_name = createGProductDto.Product_name,
                description = createGProductDto.Description,
                weight = float.Parse(createGProductDto.Package_weight),
                original_price = float.Parse(createGProductDto.Skus[0].Product_price),
                normal_stock = (int)createGProductDto.Available_stock,
                image = image,
                logistic_info = logistics_info
            };

            //TẠO SẢN PHẨM LÊN SHOPEE
            var responseCreateProductSP = await _productSPAppService.CreateProduct(long.Parse(channel_authentication.Shop_id),
                channel_authentication.Access_token,
                createProductDto);

            await Task.Delay(MILLISECONDS_DELAY);

            //TẠO BIẾN THỂ CHO SẢN PHẨM
            var requestInitTierVariationDto = RequestInitTierVariationDto(createGProductDto, responseCreateProductSP.response);

            var responseInitTierVariation = await _tierVariationAppService.InitTierVariation(long.Parse(channel_authentication.Shop_id),
                channel_authentication.Access_token,
                requestInitTierVariationDto);

            //CẬP NHẬT ID BIẾN THỂ
            responseCreateProductSP.response.model = UpdateClientSkuId(createGProductDto, responseInitTierVariation.response);

            //TẠO SẢN PHẨM VÀO DATABASE NẾU KHÔNG TẠO LỖI
            if (string.IsNullOrWhiteSpace(responseCreateProductSP.error))
            {
                await _productDomainSPAppService.CreateProductIntoDatabase(responseCreateProductSP.response,
                    createGProductDto.Client_data,
                    createGProductDto.E_channel,
                    createGProductDto.Client_product_id,
                    channel_authentication.Shop_id,
                    createGProductDto.Client_category_id);
            }
            else
            {
                throw new UserFriendlyException($"{responseCreateProductSP.message}");
            }
        }

        #endregion

        #region Sửa sản phẩm

        public async Task UpdateGProduct(CreateGProductDto createGProductDto, string channel_token)
        {
            var channel_authentication = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token
            && x.E_Channel == createGProductDto.E_channel);

            var product = await _productRepository.GetAsync(x => x.Client_Product_Id == createGProductDto.Client_category_id
            && x.E_Channel == createGProductDto.E_channel);

            var category_id = await GetChannelCategory(createGProductDto.Client_category_id, createGProductDto.E_channel);

            var image = GetImageUpdate(createGProductDto);

            RequestUpdateProductSPDto requestUpdateProductSPDto = new()
            {
                category_id = category_id,
                description = createGProductDto.Description,
                item_id = long.Parse(product.Channel_Product_Id),
                item_name = createGProductDto.Product_name,
                weight = float.Parse(createGProductDto.Package_weight),
                image = image
            };

            //update sản phẩm tiktok shop
            var responseUpdateProductSP = await _productSPAppService.Update(long.Parse(channel_authentication.Shop_id),
                channel_authentication.Access_token,
                requestUpdateProductSPDto);

            if (string.IsNullOrWhiteSpace(responseUpdateProductSP.error))
            {
                await _productDomainSPAppService.UpdateProductDatbase(createGProductDto, category_id, responseUpdateProductSP.response.images, product);
            }
            else
            {
                throw new UserFriendlyException($"{responseUpdateProductSP.message}");
            }

            //update giá cho biến thể
            if (createGProductDto.Skus != null && createGProductDto.Skus.Count > 0)
            {
                await UpdatePriceModel(createGProductDto, product, channel_authentication);
            }
        }

        #region update price & model
        private async Task UpdatePriceModel(CreateGProductDto createGProductDto, Product product, ChannelAuthentication channel_authentication)
        {
            List<PriceSPDto> listPriceSPDto = new();
            List<ModelSpDto> model_list = new();
            List<string> listClientSkuId = new();
            List<ModelResponseCreateTierVariationDto> channel_data_model_remove = new();

            RequestUpdatePriceDto requestUpdatePriceDto = new()
            {
                item_id = long.Parse(product.Channel_Product_Id)
            };

            var channel_data = _shareAppService.ConvertStringToDto<ResponseCreateProductSPDto>(product.Channel_Data);

            #region xóa model
            foreach (var item in channel_data.model.model)
            {
                if(!createGProductDto.Skus.Any(x => x.Client_sku_id == item.client_sku_id))
                {
                    DeleteModelDto deleteModelDto = new()
                    {
                        item_id = long.Parse(product.Channel_Product_Id),
                        model_id = item.model_id
                    };

                    await _modelSPAppService.Delete(long.Parse(channel_authentication.Shop_id), channel_authentication.Access_token, deleteModelDto);

                    channel_data_model_remove.Add(item);
                }
            }

            foreach(var item in channel_data_model_remove)
            {
                channel_data.model.model.Remove(item);
            }
            #endregion
            #region tạo và cập nhật model & price
            foreach (var sku in createGProductDto.Skus)
            {
                var model = channel_data.model.model.FirstOrDefault(x => x.client_sku_id == sku.Client_sku_id);

                if (model != null)
                {
                    listPriceSPDto.Add(new PriceSPDto()
                    {
                        model_id = model.model_id,
                        original_price = float.Parse(sku.Product_price)
                    });

                    channel_data.model.model.FirstOrDefault(x => x.model_id == model.model_id).price_info[0].original_price = long.Parse(sku.Product_price);
                }
                else
                {
                    listClientSkuId.Add(sku.Client_sku_id);

                    GetListModelSpDto(model_list, createGProductDto, sku, channel_data);
                }
            }
            #endregion

            if (model_list.Any())
            {
                //add model
                AddModelSPDto addModelSPDto = new()
                {
                    item_id = long.Parse(product.Channel_Product_Id),
                    model_list = model_list
                };

                var responseAddModel = await _modelSPAppService.AddModel(long.Parse(channel_authentication.Shop_id), channel_authentication.Access_token, addModelSPDto);

                //update model database
                await UpdateProductModel(listClientSkuId, responseAddModel.response, channel_data, product);
            }
            else
            {
                //update model database
                await UpdateProductModel(null, null, channel_data, product);
            }


            //update price
            requestUpdatePriceDto.price_list = listPriceSPDto;

            await _productSPAppService.UpdatePrice(long.Parse(channel_authentication.Shop_id),
                channel_authentication.Access_token,
                requestUpdatePriceDto);
        }

        #region update model

        private async Task UpdateProductModel(List<string> listClientSkuId,
            ResponseAddModelDto responseAddModel,
            ResponseCreateProductSPDto channel_data,
            Product product)
        {
            if (responseAddModel != null && responseAddModel.model.Any())
            {
                for (var i = 0; i < responseAddModel.model.Count; i++)
                {
                    channel_data.model.model.Add(new ModelResponseCreateTierVariationDto()
                    {
                        client_sku_id = listClientSkuId[i],
                        model_id = responseAddModel.model[i].model_id,
                        price_info = responseAddModel.model[i].price_info,
                        seller_stock = responseAddModel.model[i].seller_stock,
                        stock_info = responseAddModel.model[i].stock_info,
                        tier_index = responseAddModel.model[i].tier_index
                    });
                }
            }

            product.Channel_Data = ShareAppService.ConvertDtoToString(channel_data);

            await _productRepository.UpdateAsync(product);
        }

        #endregion

        private static List<ModelSpDto> GetListModelSpDto(List<ModelSpDto> model_list,
            CreateGProductDto createGProductDto,
            GSkuDto sku,
            ResponseCreateProductSPDto channel_data)
        {
            List<int> tier_index = new();

            foreach (var attribute in sku.Sales_attributes)
            {
                var tier_variation = channel_data.model.tier_variation.FirstOrDefault(x => x.name == attribute.Client_attribute_name);

                int index = tier_variation.option_list.FindIndex(x => x.option == attribute.Custom_value);

                tier_index.Add(index);
            }

            model_list.Add(new ModelSpDto()
            {
                tier_index = tier_index,
                normal_stock = (int)createGProductDto.Available_stock,
                original_price = float.Parse(sku.Product_price)
            });

            return model_list;
        }

        #endregion

        #endregion

        #region Sửa giá

        public async Task UpdateGProductPrice(string channel_token, EChannel e_channel, string client_product_id, [FromBody] List<GUpdatePriceDto> listSku)
        {
            var channel_authentication = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token
            && x.E_Channel == e_channel);

            var product = await _productRepository.GetAsync(x => x.Client_Product_Id == client_product_id
            && x.E_Channel == e_channel);

            var channel_data = _shareAppService.ConvertStringToDto<ResponseCreateProductSPDto>(product.Channel_Data);

            List <PriceSPDto> price_list = new();

            foreach (var sku in listSku)
            {
                var model = channel_data.model.model.FirstOrDefault(x => x.client_sku_id == sku.Client_sku_id);

                price_list.Add(new PriceSPDto()
                {
                    model_id = model.model_id,
                    original_price = float.Parse(sku.Product_price)
                });

                channel_data.model.model.FirstOrDefault(x => x.model_id == model.model_id).price_info[0].original_price = long.Parse(sku.Product_price);
            }

            RequestUpdatePriceDto requestUpdatePriceDto = new()
            {
                item_id = long.Parse(product.Channel_Product_Id),
                price_list = price_list
            };

            var responseUpdatePrice = await _productSPAppService.UpdatePrice(long.Parse(channel_authentication.Shop_id), 
                channel_authentication.Access_token, 
                requestUpdatePriceDto);

            if (string.IsNullOrWhiteSpace(responseUpdatePrice.error))
            {
                product.Channel_Data = ShareAppService.ConvertDtoToString(channel_data);

                await _productRepository.UpdateAsync(product);
            }
            else
            {
                throw new UserFriendlyException(responseUpdatePrice.error);
            }
        }

        #endregion

        #region Helper function

        private async Task<long> GetChannelCategory(string client_category_id, EChannel e_channel)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Client_category_Id == client_category_id && x.E_Channel == e_channel);

            return long.Parse(category.Category_Id);
        }

        private static RequestInitTierVariationDto RequestInitTierVariationDto(CreateGProductDto createGProductDto, ResponseCreateProductSPDto responseCreateProductSP)
        {
            List<TierVariationDto> tier_variations = new();
            List<ModelSpDto> models = new();

            for (int i = 0; i < createGProductDto.Skus.Count; i++)
            {
                #region tier_variation

                for (int j = 0; j < createGProductDto.Skus[i].Sales_attributes.Count; j++)
                {
                    List<OptionDto> option_list = new();

                    if (!tier_variations.Any(x => x.name == createGProductDto.Skus[i].Sales_attributes[j].Client_attribute_name))
                    {
                        option_list.Add(new OptionDto() { option = createGProductDto.Skus[i].Sales_attributes[j].Custom_value });

                        tier_variations.Add(new TierVariationDto()
                        {
                            name = createGProductDto.Skus[i].Sales_attributes[j].Client_attribute_name,
                            option_list = option_list
                        });
                    }
                    else
                    {
                        tier_variations.FirstOrDefault(x => x.name == createGProductDto.Skus[i].Sales_attributes[j].Client_attribute_name)
                            .option_list.Add(new OptionDto() { option = createGProductDto.Skus[i].Sales_attributes[j].Custom_value });
                    }
                }

                #endregion

                #region model

                List<int> tier_index = new();

                for (int j = 0; j < createGProductDto.Skus[i].Sales_attributes.Count; j++)
                {
                    var tier_variation = tier_variations.FirstOrDefault(x => x.name == createGProductDto.Skus[i].Sales_attributes[j].Client_attribute_name);

                    var index = tier_variation.option_list.FindIndex(x => x.option == createGProductDto.Skus[i].Sales_attributes[j].Custom_value);

                    tier_index.Add(index);
                }

                var model = new ModelSpDto()
                {
                    original_price = float.Parse(createGProductDto.Skus[i].Product_price),
                    normal_stock = (int)createGProductDto.Available_stock,
                    tier_index = tier_index
                };

                models.Add(model);

                #endregion
            }

            RequestInitTierVariationDto requestInitTierVariationDto = new()
            {
                item_id = responseCreateProductSP.item_id,
                model = models,
                tier_variation = tier_variations
            };

            return requestInitTierVariationDto;
        }


        private async Task<List<LogisticInfoCreateProductSPDto>> GetLogisticInfosAsync(ChannelAuthentication channel_authentication)
        {
            List<LogisticInfoCreateProductSPDto> logistics_info = new();

            var logistic_info_sp = await _logisticsSPAppService.GetChannels(long.Parse(channel_authentication.Shop_id), channel_authentication.Access_token);

            var logistics_channel = logistic_info_sp.response.logistics_channel_list.FirstOrDefault(x => x.logistics_channel_name == LOGISTICS_CHANNEL_NAME
            && x.enabled);

            if (logistics_channel == null)
            {
                logistics_channel = logistic_info_sp.response.logistics_channel_list.FirstOrDefault(x => x.enabled);
            }

            logistics_info.Add(new LogisticInfoCreateProductSPDto()
            {
                enabled = logistics_channel.enabled,
                logistic_id = logistics_channel.logistics_channel_id
            });

            return logistics_info;
        }

        private static ImageCreateProductSPDto GetImagesCreate(CreateGProductDto createGProductDto)
        {
            ImageCreateProductSPDto image = new()
            {
                image_id_list = createGProductDto.Image_ids
            };

            return image;
        }

        private async Task HandleModelCreateAsync(CreateGProductDto createGProductDto)
        {
            var isExistClientProductId = await _productRepository.AnyAsync(x => x.Client_Product_Id == createGProductDto.Client_product_id
            && x.E_Channel == createGProductDto.E_channel);

            if (isExistClientProductId)
            {
                throw new UserFriendlyException($"Đã tồn tại sản phẩm này! {createGProductDto.Product_name}. ID: {createGProductDto.Client_product_id}");
            }
        }

        private static ResponseCreateTierVariationDto UpdateClientSkuId(CreateGProductDto createGProductDto, ResponseCreateTierVariationDto response)
        {
            for (var i = 0; i < response.model.Count; i++)
            {
                response.model[i].client_sku_id = createGProductDto.Skus[i].Client_sku_id;
            }

            return response;
        }

        private static UpdateImageDto GetImageUpdate(CreateGProductDto createGProductDto)
        {
            UpdateImageDto updateImageDto = new()
            {
                image_id_list = createGProductDto.Image_ids
            };

            return updateImageDto;
        }

        #endregion

        #region Background Job

        [RemoteService(false)]
        public void CreateGProductSPBackgroundJob(CreateGProductDto createGProductDto, string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductBackgroundJobSPAppService>(x => x.CreateGProduct(createGProductDto, channel_token));
        }

        [RemoteService(false)]
        public void UpdateGProductSPBackgroundJob(CreateGProductDto createGProductDto, string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductBackgroundJobSPAppService>(x => x.UpdateGProduct(createGProductDto, channel_token));
        }

        [RemoteService(false)]
        public void UpdateGProductPriceSPBackgroundJob(string channel_token, EChannel e_channel, string client_product_id, [FromBody] List<GUpdatePriceDto> listSku)
        {
            _backgroundJobClient.Enqueue<IProductBackgroundJobSPAppService>(x => x.UpdateGProductPrice(channel_token, e_channel, client_product_id, listSku));
        }

        #endregion
    }
}
