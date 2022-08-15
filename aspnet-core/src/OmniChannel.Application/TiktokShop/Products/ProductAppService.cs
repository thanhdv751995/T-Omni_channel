using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.CreateProducts;
using OmniChannel.DeleteProducts;
using OmniChannel.DeleteProductsc;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Images;
using OmniChannel.MultiTenancy;
using OmniChannel.ProductDetail;
using OmniChannel.Shares;
using OmniChannel.SKUs;
using OmniChannel.Stocks;
using OmniChannel.UpdateProducts;
using Volo.Abp;
using OmniChannel.TiktokShop.CreateProducts;
using OmniChannel.Channels;
using OmniChannel.General.GProducts;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Categories;
using OmniChannel.TiktokShop.Attributes;
using OmniChannel.Attributes;
using OmniChannel.TiktokShop.ProductDetail;
using Hangfire;
using OmniChannel.General.LinkedProducts;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using OmniChannel.TiktokShop.ProductImages;
using OmniChannel.SalesAttributes;
using OmniChannel.ProductStatuss;

namespace OmniChannel.Products
{
    [RemoteService(true)]
    public class ProductAppService : OmniChannelAppService, IProductAppService
    {
        private readonly string TiktokShopApiGetListProduct = OmniChannelConsts.TiktokShopAPIGetListProduct;
        private readonly string TiktokShopDefaultCustomValue = OmniChannelConsts.TiktokShopDefaultCustomValue;
        private readonly string TiktokShopDefaultColorCustomValue = OmniChannelConsts.TiktokShopDefaultColorCustomValue;
        private readonly string TiktokShopDefaultSizeCustomValue = OmniChannelConsts.TiktokShopDefaultSizeCustomValue;
        private readonly string TiktokShopDefaultColor = OmniChannelConsts.TiktokShopDefaultColor;
        private readonly string TiktokShopDefaultSize = OmniChannelConsts.TiktokShopDefaultSize;
        private readonly string TiktokShopAPIGetProductDetail = OmniChannelConsts.TiktokShopAPIGetProductDetail;
        private readonly string TiktokShopApiActiveProduct = OmniChannelConsts.TiktokShopApiActiveProduct;
        private readonly string TiktokShopApiDeactiveProduct = OmniChannelConsts.TiktokShopApiDeactiveProduct;
        private readonly string TiktokShopApiRecoverDeletedProduct = OmniChannelConsts.TiktokShopApiRecoverDeletedProduct;
        private readonly string TiktokShopApiUpdatePrice = OmniChannelConsts.TiktokShopApiUpdatePrice;
        private readonly string TiktokShopApiUpdateStock = OmniChannelConsts.TiktokShopApiUpdateStock;
        private readonly string TiktokShopCreateProduct = OmniChannelConsts.TiktokShopCreateProduct;
        private readonly string NOTFOUND = OmniChannelErrorConsts.NOTFOUND;
        private readonly List<string> ListAttributeColorDefault = new(new string[] { "Xanh", "Đen", "Nâu", "Hồng", "Đỏ", "Vàng", "Tím", "Bạc", "Xám", "Cam", "Xanh Lá Cây" });
        private readonly List<string> ListAttributeSizeDefault = new(new string[] { "S", "M", "L", "XL", "XXL", "XXXL" });
        private readonly string HttpResponseSuccessMessage = OmniChannelConsts.HttpResponseSuccessMessage;

        private readonly ShareAppService _shareAppService;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ProductImageManager _productImageManager;
        private readonly ProductManager _productManager;
        private readonly IProductRepository _productRepository;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly AttributeAppService _attributeAppService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;
        public ProductAppService(ShareAppService shareAppService,
                                    ProductManager productManager,
                                    IProductRepository productRepository,
                                    IChannelAuthenticationRepository channelAuthenticationRepository,
                                    ICategoryRepository categoryRepository,
                                    IAttributeRepository attributeRepository,
                                    IProductImageRepository productImageRepository,
                                    ProductImageManager productImageManager,
                                    ChannelAuthenticationAppService channelAuthenticationAppService,
                                    AttributeAppService attributeAppService,
                                    IBackgroundJobClient backgroundJobClient,
                                    IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _productManager = productManager;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _productImageManager = productImageManager;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
            _attributeAppService = attributeAppService;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        [RemoteService(true)]
        public async Task<ResponseDataDto<ReponseProductDetailTikTokShopDto>> GetProductDetail(string product_id, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"product_id={product_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopAPIGetProductDetail, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ReponseProductDetailTikTokShopDto>>(httpResponseMessage);
        }

        [RemoteService(true)]
        /// <summary>
        /// Lấy mô tả chi tiết sản phẩm
        /// </summary>
        /// <param name="searchListProductDto"></param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseDataDto<ResponseListProductDto>> Products(RequestSearchListProductDto searchListProductDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListProduct, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, searchListProductDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseListProductDto>>(httpResponseMessage);
        }

        [RemoteService(true)]
        //tạo sản phẩm
        /// <summary>
        /// Tạo sản phẩm
        /// </summary>
        /// <param name="createProductDto"> <p> - product_certifications : Giấy chứng nhận sản phẩm</p> 
        ///                                 <p> - is_cod_open : Hiệu lực sản phẩm </p> 
        ///                                 <p> - sales_attributes : thuộc tính giá </p>
        ///                                 <p> - stock_infos : Thông tin kho </p>
        ///                                 <p> - delivery_service_ids : Phương thức vận chuyển </p>
        ///                                 <p> - product_attributes : thuộc tính sản phẩm</p>
        ///                                 <p> - sales-sku : đơn vị lưu giữ kho (cá nhân / đơn vị) </p>
        ///                                 <p> - size_chart : ảnh size sản phẩm </p>
        /// </param>
        /// <param name="channel_token"></param>
        /// <returns></returns>
        public async Task<ResponseCreateProductDto> CreateProductTiktokShop(CreateProductDto createProductDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopCreateProduct, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, createProductDto);

            var response = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseCreateProductDto>(httpResponseMessage);

            _backgroundJobClient.Enqueue<IProductAppService>(x => x.UpDateProductStatus(response.data.product_id, channel_token));

            return response;
        }

        public async Task UpdateProduct(UpdateProductDto updateProductDto, string channel_token, string client_data)
        {
            var response = await UpdateProductTikTokShop(updateProductDto, channel_token);

            if (response.message == HttpResponseSuccessMessage)
            {
                var product = await _productRepository.GetAsync(x => x.Channel_Product_Id == response.data.product_id);

                var dataChannel = ObjectMapper.Map<UpdateProductDto, DataChannelProductDto>(updateProductDto);

                var dataUpdate = ShareAppService.ConvertObjToString(dataChannel);

                product.Channel_Data = dataUpdate;

                product.Client_Data = client_data;

                await _productRepository.UpdateAsync(product);
            }
            else
            {
                throw new UserFriendlyException($"{response.message}");
            }
        }

        [RemoteService(false)]
        [UnitOfWork]
        public async Task CreateProductIntoDatabase(CreateProductDto createProductDto, ResponseCreateProductDto responseCreateProductDto, string client_data, EChannel eChannel, string client_product_id, string shop_id, string category_id)
        {
            var dataChannel = ObjectMapper.Map<CreateProductDto, DataChannelProductDto>(createProductDto);

            var channel_data = ShareAppService.ConvertObjToString(dataChannel);

            var product = _productManager.CreateAsync(shop_id,
                responseCreateProductDto.data.product_id,
                client_product_id,
                category_id,
                eChannel,
                channel_data,
                client_data,
                true);

            await _productRepository.InsertAsync(product);
        }

        [RemoteService(false)]
        public async Task UpdateSkuId(Guid product_id)
        {
            var productInDatabase = await _productRepository.GetAsync(x => x.Id == product_id);

            var channelAuthentication = await _channelAuthenticationRepository.GetAsync(x => x.Shop_id == productInDatabase.Shop_Id);

            var productTiktokShop = await GetProductDetail(productInDatabase.Channel_Product_Id, channelAuthentication.Channel_token);

            var productChannelDataDto = _shareAppService.ConvertStringToDto<DataChannelProductDto>(productInDatabase.Channel_Data);

            if (productInDatabase != null)
            {
                for (var i = 0; i < productChannelDataDto.Skus.Count; i++)
                {
                    productChannelDataDto.Skus[i].Id = productTiktokShop.Data.Skus[i].id;
                }
            }

            productInDatabase.Channel_Data = ShareAppService.ConvertDtoToString(productChannelDataDto);

            await _productRepository.UpdateAsync(productInDatabase);
        }

        //Lấy category dựa theo tên của sản phẩm
        private async Task<Category> GetCategoryByProductName(CreateGProductDto createGProductDto)
        {
            var listCategory = await _categoryRepository.GetListAsync(x => x.E_Channel == createGProductDto.E_channel && x.Client_category_Id == createGProductDto.Client_category_id);

            //không tìm thấy category
            if (listCategory.Count == 0)
            {
                throw new UserFriendlyException($"{NOTFOUND}", nameof(createGProductDto.Client_category_id));
            }

            Category category = null;

            //nếu tìm thấy nhiều loại đã map với client category id
            if (listCategory.Count > 1)
            {
                var nameSplit = createGProductDto.Product_name.Split(" ").ToList();

                //tìm loại theo tên sản phẩm: VD: áo thun ..., loại: áo thun nam (for(áo, thun, nam)) => áo thun == áo thun
                for (var i = nameSplit.Count - 1; i > 0; i--)
                {
                    if (listCategory.Any(x => x.Display_name.ToLower().Contains(createGProductDto.Product_name.ToLower().Replace(nameSplit[i], ""))))
                    {
                        category = listCategory.FirstOrDefault(x => x.Display_name.Contains(createGProductDto.Product_name.Replace(nameSplit[i], "")));
                    }
                }

                //không tìm thấy => lấy mặc định là thứ 0
                if (category == null)
                {
                    category = listCategory[0];
                }
            }
            //duy nhất 1 category
            else
                category = listCategory[0];

            return category;
        }

        private async Task<Tuple<string, string>> GetWareHouseIdShopId(CreateGProductDto createGProductDto, string channel_token)
        {
            var channel_Authen = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token && x.E_Channel == createGProductDto.E_channel);

            var warehouse_id = channel_Authen.Warehouse_list.FirstOrDefault(x => x.Warehouse_type == 1).Warehouse_id;

            return new Tuple<string, string>(warehouse_id, channel_Authen.Shop_id);
        }

        [UnitOfWork]
        private async Task<Tuple<List<CreateImagesIdDto>,
            List<CreateSkusProductDto>,
            Category,
            Tuple<string, string>>> ConvertToTiktokShopDto(CreateGProductDto createGProductDto, string channel_token)
        {
            var warehouseId_shopId = await GetWareHouseIdShopId(createGProductDto, channel_token);

            Category category = await GetCategoryByProductName(createGProductDto);

            List<CreateImagesIdDto> createImagesIdDtos = new();

            List<CreateSkusProductDto> createSkusProductDtos = new();

            //chuyển data dto theo dạng tiktok shop image dto
            foreach (var imageId in createGProductDto.Image_ids)
            {
                createImagesIdDtos.Add(new CreateImagesIdDto()
                {
                    id = imageId,
                });
            }

            //lấy số lượng mỗi sản phẩm = tổng sản phẩm / số sản phẩm
            //decimal specific_stock = Math.Floor((decimal)createGProductDto.Available_stock.Value / (decimal)createGProductDto.Skus.Count);

            //lấy attributes của tiktok shop bằng category id
            var attributesTiktokShop = await _attributeAppService.GetListAttributeTiktokShop(category.Category_Id);

            for (var i = 0; i < createGProductDto.Skus.Count; i++)
            {
                //báo lỗi nếu số thuộc tính user nhập  > số thuộc tính của tiktok shop
                if (attributesTiktokShop.Data.Attributes.Count < createGProductDto.Skus[i].Sales_attributes.Count)
                {
                    throw new UserFriendlyException($"Danh mục này chỉ có {attributesTiktokShop.Data.Attributes.Count} thuộc tính của tiktok shop: " +
                        $"{string.Join(",", attributesTiktokShop.Data.Attributes.Select(x => x.Name))}");
                }

                List<CreateStockInfosProductDto> stock_infos = new();

                //nếu biến thể > 1 thì biến thể đầu tiên gán số lượng còn lại thì bằng 0
                if (i == 0)
                {
                    stock_infos.Add(new CreateStockInfosProductDto()
                    {
                        Available_stock = createGProductDto.Available_stock.Value,
                        Warehouse_id = warehouseId_shopId.Item1
                    });
                }
                else
                {
                    stock_infos.Add(new CreateStockInfosProductDto()
                    {
                        Available_stock = 0,
                        Warehouse_id = warehouseId_shopId.Item1
                    });
                }

                List<SalesAttributesDto> sales_attributes = new();

                foreach (var sales_attribute in createGProductDto.Skus[i].Sales_attributes)
                {
                    var attributes = await _attributeRepository.GetListAsync(x => x.Client_attribute_id == sales_attribute.Client_attribute_id
                    && x.E_Channel == createGProductDto.E_channel);

                    if (attributes.Count > 0)
                    {
                        if (attributes.Count == 1)
                        {
                            //nếu attribute của tiktok shop có tồn tại attribute trong CSDL
                            if (attributesTiktokShop.Data.Attributes.Any(x => x.Id == attributes[0].Attribute_id))
                            {
                                sales_attributes.Add(new SalesAttributesDto()
                                {
                                    Attribute_id = attributes[0].Attribute_id,
                                    Custom_value = sales_attribute.Custom_value
                                });
                            }
                        }
                        else
                        {
                            //quy chuẩn tên attribute thông tin người dùng nhập thành quy chuẩn tên attribute của tiktok shop 
                            if (sales_attribute.Client_attribute_name.ToLower().Contains("size") || sales_attribute.Client_attribute_name.ToLower().Contains("kích cỡ"))
                            {
                                sales_attribute.Client_attribute_name = TiktokShopDefaultSize;//Kíchcỡ (tiktok shop)
                            }

                            //nếu có tồn tại attribute người dùng nhập theo tên
                            if (attributes.Any(x => x.Name.ToLower().Contains(sales_attribute.Client_attribute_name.ToLower())))
                            {
                                TiktokShop.Attributes.Attribute attribute = attributes.FirstOrDefault(x => x.Name.ToLower().Contains(sales_attribute.Client_attribute_name.ToLower()));

                                sales_attributes.Add(new SalesAttributesDto()
                                {
                                    Attribute_id = attribute.Attribute_id,
                                    Custom_value = sales_attribute.Custom_value
                                });
                            }
                            else
                            {
                                TiktokShop.Attributes.Attribute attribute = null;

                                if (ListAttributeColorDefault.Any(x => x.ToLower() == sales_attribute.Custom_value.ToLower()))
                                {
                                    attribute = attributes.FirstOrDefault(x => x.Name.ToLower().StartsWith("màu sắc"));
                                }

                                if (ListAttributeSizeDefault.Any(x => x.ToLower() == sales_attribute.Custom_value.ToLower()))
                                {
                                    attribute = attributes.FirstOrDefault(x => x.Name.ToLower().Contains("kíchcỡ"));
                                }

                                if (attribute != null)
                                {
                                    sales_attributes.Add(new SalesAttributesDto()
                                    {
                                        Attribute_id = attribute.Attribute_id,
                                        Custom_value = sales_attribute.Custom_value
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException($"{NOTFOUND}", nameof(sales_attribute.Client_attribute_id));
                    }
                }

                createSkusProductDtos.Add(new CreateSkusProductDto()
                {
                    Original_price = createGProductDto.Skus[i].Product_price,
                    Client_sku_id = createGProductDto.Skus[i].Client_sku_id,
                    Sales_attributes = sales_attributes,
                    Stock_infos = stock_infos
                });
            }

            return new Tuple<List<CreateImagesIdDto>, List<CreateSkusProductDto>, Category, Tuple<string, string>>(createImagesIdDtos, createSkusProductDtos, category, warehouseId_shopId);
        }

        private List<SalesAttributesDto> GetSalesAttributesDefault(ResponseDataDto<DataListAttributeDto> attributesTiktokShop)
        {
            List<SalesAttributesDto> sales_attributes = new();

            string attribute_id;
            string custom_value;

            if (attributesTiktokShop.Data.Attributes.Any(x => x.Name == TiktokShopDefaultSize))
            {
                attribute_id = attributesTiktokShop.Data.Attributes.FirstOrDefault(x => x.Name == TiktokShopDefaultSize).Id;
                custom_value = TiktokShopDefaultSizeCustomValue;
            }
            else if (attributesTiktokShop.Data.Attributes.Any(x => x.Name == TiktokShopDefaultColor))
            {
                attribute_id = attributesTiktokShop.Data.Attributes.FirstOrDefault(x => x.Name == TiktokShopDefaultColor).Id;
                custom_value = TiktokShopDefaultColorCustomValue;
            }
            else
            {
                attribute_id = attributesTiktokShop.Data.Attributes.FirstOrDefault(x => x.Values == null).Id;
                custom_value = TiktokShopDefaultCustomValue;
            }

            sales_attributes.Add(new SalesAttributesDto()
            {
                Attribute_id = attribute_id,
                Custom_value = custom_value
            });

            return sales_attributes;
        }

        [RemoteService(false)]
        public void CreateGProductBackgroundJob(CreateGProductDto createGProductDto, string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductAppService>(x => x.CreateGProduct(createGProductDto, channel_token));

        }

        [RemoteService(false)]
        public async Task CreateGProduct(CreateGProductDto createGProductDto, string channel_token)
        {
            var gProductDto = await ConvertToTiktokShopDto(createGProductDto, channel_token);

            CreateProductDto createProductDto = new()
            {
                Product_name = createGProductDto.Product_name,
                Description = createGProductDto.Description,
                Is_cod_open = false,
                Package_weight = createGProductDto.Package_weight,
                Images = gProductDto.Item1,
                Category_id = gProductDto.Item3.Category_Id,
                Skus = gProductDto.Item2
            };

            var responseCreateTiktokShop = await CreateProductTiktokShop(createProductDto, channel_token);

            if (responseCreateTiktokShop.message == HttpResponseSuccessMessage)
            {
                await CreateProductIntoDatabase(createProductDto,
                    responseCreateTiktokShop,
                    createGProductDto.Client_data,
                    createGProductDto.E_channel,
                    createGProductDto.Client_product_id,
                    gProductDto.Item4.Item2,
                    createGProductDto.Client_category_id);
            }
            else
            {
                throw new UserFriendlyException($"{responseCreateTiktokShop.message}");
            }
        }

        [RemoteService(false)]
        public void UpdateGProductBackgroundJob(CreateGProductDto createGProductDto, string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductAppService>(x => x.UpdateGProduct(createGProductDto, channel_token));
        }

        [RemoteService(false)]
        public async Task UpdateGProduct(CreateGProductDto createGProductDto, string channel_token)
        {
            var gProductDto = await ConvertToTiktokShopDto(createGProductDto, channel_token);
            var listChannelAuthen = await _channelAuthenticationRepository.GetListAsync(x => x.E_Channel == createGProductDto.E_channel);

            var product = await _productRepository.GetAsync(x => x.Client_Product_Id == createGProductDto.Client_product_id);

            UpdateProductDto productDto = new()
            {
                Product_id = product.Channel_Product_Id,
                Product_name = createGProductDto.Product_name,
                Description = createGProductDto.Description,
                Is_cod_open = false,
                Package_weight = createGProductDto.Package_weight,
                Images = gProductDto.Item1,
                Category_id = gProductDto.Item3.Category_Id,
                Skus = gProductDto.Item2
            };

            await UpdateProduct(productDto, channel_token, createGProductDto.Client_data);
        }

        [RemoteService(false)]
        public async Task ActivateDeActiveGProduct(EChannel e_channel, string channel_token, bool is_active, ActivateGProductDto activateGProductDto)
        {
            RequestProductIdsDto requestProductIdsDto = new();
            List<string> product_ids = new();

            foreach (var client_product_id in activateGProductDto.client_product_ids)
            {
                var product = await _productRepository.GetAsync(x => x.E_Channel == e_channel && x.Client_Product_Id == client_product_id);

                product_ids.Add(product.Channel_Product_Id);

                product.IsActive = is_active;

                await _productRepository.UpdateAsync(product);
            }

            requestProductIdsDto.product_ids = product_ids;

            if (is_active)
            {
                await ActivateProduct(requestProductIdsDto, channel_token);
            }
            else
            {
                await DeactivateProduct(requestProductIdsDto, channel_token);
            }
        }

        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseFailedProductIdsDto>> ActivateProduct(RequestProductIdsDto requestProductIdsDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiActiveProduct, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token,
                channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestProductIdsDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseFailedProductIdsDto>>(httpResponseMessage);
        }

        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseFailedProductIdsDto>> DeactivateProduct(RequestProductIdsDto requestProductIdsDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiDeactiveProduct, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestProductIdsDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseFailedProductIdsDto>>(httpResponseMessage);
        }


        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseFailedProductIdsDto>> RecoverDeletedProduct(RequestProductIdsDto requestProductIdsDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiRecoverDeletedProduct, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.POST, requestProductIdsDto);

            var resultJson = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseFailedProductIdsDto>>(httpResponseMessage);

            return resultJson;
        }

        [RemoteService(false)]
        public void UpdateGProductPriceBackgroundJob(EChannel e_channel, string client_product_id, string channel_token, [FromBody] List<GUpdatePriceDto> listSku)
        {
            _backgroundJobClient.Enqueue<IProductAppService>(x => x.UpdateGProductPrice(e_channel, client_product_id, channel_token, listSku));
        }

        public async Task UpDateProductStatus(string channel_product_id, string channel_token)
        {
            var productTiktokShop = await GetProductDetail(channel_product_id, channel_token);
            while (productTiktokShop.Data.Product_status == 2)
            {
                //kiểm tra sản phẩm tiktok shop sau 5s
                Task.Delay(10).Wait();
                productTiktokShop = await GetProductDetail(channel_product_id, channel_token);
            }

            var product = await _productRepository.GetAsync(x => x.Channel_Product_Id == channel_product_id);

            var channel_data = _shareAppService.ConvertStringToDto<DataChannelProductDto>(product.Channel_Data);

            channel_data.Product_status = productTiktokShop.Data.Product_status;

            var channel_data_string = ShareAppService.ConvertDtoToString(channel_data);

            product.Channel_Data = channel_data_string;

            await _productRepository.UpdateAsync(product);
        }

        [RemoteService(false)]
        public async Task UpdateGProductPrice(EChannel e_channel, string client_product_id, string channel_token, [FromBody] List<GUpdatePriceDto> listSku)
        {
            var product = await _productRepository.GetAsync(x => x.Client_Product_Id == client_product_id && x.E_Channel == e_channel);
            var productTiktokShop = await GetProductDetail(product.Channel_Product_Id, channel_token);

            var productChannelDataDto = _shareAppService.ConvertStringToDto<DataChannelProductDto>(product.Channel_Data);

            //trạng thái của sản phẩm tiktok shop là pending
            while (productTiktokShop.Data.Product_status == 2)
            {
                //kiểm tra sản phẩm tiktok shop sau 5s
                Task.Delay(5).Wait();
                productTiktokShop = await GetProductDetail(product.Channel_Product_Id, channel_token);
            }

            //trạng thái sản phẩm tiktok shop là active
            if (productTiktokShop.Data.Product_status == 4)
            {
                RequestUpdatePriceDto requestUpdatePriceDto = new();
                List<OriginalPriceDto> skus = new();

                foreach (GUpdatePriceDto g_sku in listSku)
                {
                    var sku = productChannelDataDto.Skus.Find(x => x.Client_sku_id == g_sku.Client_sku_id);

                    //chuyển sang dto của update price tiktok shop
                    OriginalPriceDto originalPriceDto = new()
                    {
                        original_price = g_sku.Product_price,
                        id = sku.Id
                    };

                    skus.Add(originalPriceDto);

                    productChannelDataDto.Skus.Find(x => x.Client_sku_id == g_sku.Client_sku_id).Original_price = g_sku.Product_price;
                }

                requestUpdatePriceDto.product_id = product.Channel_Product_Id;

                requestUpdatePriceDto.skus = skus;

                var httpResponse = await UpdatePrice(requestUpdatePriceDto, channel_token);

                if (httpResponse.Message == HttpResponseSuccessMessage)
                {
                    product.Channel_Data = ShareAppService.ConvertDtoToString(productChannelDataDto);

                    await _productRepository.UpdateAsync(product);
                }
                else
                {
                    throw new UserFriendlyException(httpResponse.Message);
                }
            }
        }

        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseFailedKkuIdsDto>> UpdatePrice(RequestUpdatePriceDto requestUpdatePriceDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiUpdatePrice, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.PUT, requestUpdatePriceDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseFailedKkuIdsDto>>(httpResponseMessage);
        }

        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseFailedSkusDto>> UpdateStock(RequestUpdateStockDto requestUpdateStockDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiUpdateStock, _configuration["AppTiktokShopSetting:App_key"], channel.Access_token, channel.Shop_id,
                _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.PUT, requestUpdateStockDto);

            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseFailedSkusDto>>(httpResponseMessage);
        }

        [RemoteService(false)]
        // chỉnh sửa sản phẩm
        public async Task<ResponseCreateProductDto> UpdateProductTikTokShop(UpdateProductDto updateProductDto, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopCreateProduct, _configuration["AppTiktokShopSetting:App_key"],
                channel.Access_token, channel.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.PUT, updateProductDto);

            var resultJson = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseCreateProductDto>(httpResponseMessage);

            return resultJson;
        }

        [RemoteService(false)]
        public void DeleteGProductBackgroundJob(List<string> product_ids, string channel_token)
        {
            _backgroundJobClient.Enqueue<IProductAppService>(x => x.DeleteProduct(product_ids, channel_token));
        }

        [RemoteService(false)]
        // xóa sản phẩm
        public async Task DeleteProduct([FromBody] List<string> client_product_ids, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var channel_product_ids = new List<string>();

            foreach (var client_product_id in client_product_ids)
            {
                var product = await _productRepository.GetAsync(x => x.Client_Product_Id == client_product_id && x.E_Channel == channel.E_Channel);

                channel_product_ids.Add(product.Channel_Product_Id);
            }

            DeleteProductDto deleteProductDto = new()
            {
                product_ids = channel_product_ids
            };

            await DeleteProductTikTokShop(deleteProductDto, channel.Access_token, channel.Shop_id);

            foreach (var product_id in client_product_ids)
            {
                await _productRepository.DeleteAsync(x => x.Client_Product_Id == product_id && x.E_Channel == channel.E_Channel);
            }
        }

        [RemoteService(false)]
        public async Task DeleteProductTikTokShop([FromBody] DeleteProductDto deleteProductDto, string access_token, string shop_id)
        {
            var url = _shareAppService.GetTiktokShopUrl(TiktokShopCreateProduct, _configuration["AppTiktokShopSetting:App_key"], access_token, shop_id,
                _configuration["AppTiktokShopSetting:App_secret"], new List<string>());

            await HttpClientAppService.GetResponseMessage(url, EHttpMethod.DELETEFORMBODY, deleteProductDto);
        }



        [RemoteService(false)]
        //kiểm tra danh sách sản phẩm trong tiktokShop và database nếu có thì update chưa có thì create
        ///
        public async Task GetListProductRepeat()
        {
            RequestSearchListProductDto searchListProductDto = new()
            {
                page_number = 1,
                page_size = 100
            };
            var listChannel = await _channelAuthenticationAppService.GetListChannelTiktokShop();
            if (listChannel != null)
            {
                foreach (var channel in listChannel)
                {
                    var listProduct = await Products(searchListProductDto, channel.Channel_token);// product in tiktok-shop
                    foreach (var product in listProduct.Data.Products)
                    {
                        var productTTSDetail = await GetProductDetail(product.Id, channel.Channel_token);

                        var productDB = await _productRepository.FindAsync(x => x.Channel_Product_Id == product.Id);

                        List<CreateSkusProductDto> skusProductsDTo = new();

                        if (productTTSDetail.Data.Skus != null)
                        {
                           

                            foreach (var sku in productTTSDetail.Data.Skus)
                            {
                                List<CreateStockInfosProductDto> listStock = ObjectMapper.Map<List<StockInfosDtoCreateProduct>, List<CreateStockInfosProductDto>>(sku.stock_infos);

                                //foreach (var stock in sku.stock_infos)
                                //{
                                //    CreateStockInfosProductDto createStockInfosProductDto = new()
                                //    {
                                //        Warehouse_id = stock.warehouse_id,
                                //        Available_stock = stock.available_stock,
                                //    };
                                //    listStock.Add(createStockInfosProductDto);
                                //}

                                List<SalesAttributesDto> listSaleAtributeDto = new();
                                CreateSkusProductDto skusDto = new()
                                {
                                    Id = sku.id,
                                    Sales_attributes = listSaleAtributeDto,
                                    Seller_sku = sku.seller_sku,
                                    Original_price = sku.price?.original_price,
                                    Stock_infos = listStock,
                                };
                                if (sku.sales_attributes != null)
                                {
                                    foreach (var atribute in sku.sales_attributes)
                                    {
                                        SalesAttributesDto saleAtributeDto = new()
                                        {
                                            Id = GuidGenerator.Create(),
                                            Attribute_id = atribute.id,
                                            Custom_value = atribute.value_name
                                        };
                                        listSaleAtributeDto.Add(saleAtributeDto);
                                    }
                                }

                                skusProductsDTo.Add(skusDto);

                            }
                        }

                        List<CreateImagesIdDto> imagesId = new();

                        if (productTTSDetail.Data.Images != null)
                        {
                                var isAnyImage = await _productImageRepository.AnyAsync(x => x.Img_id == productTTSDetail.Data.Images[0].Id);
                                var imageDetailProductId = productTTSDetail.Data.Images[0].Id;

                                if (!isAnyImage)
                                {
                                        var image = _productImageManager.CreateAsync(imageDetailProductId, productTTSDetail.Data.Images[0].Url_list[0], 1);
                                        await _productImageRepository.InsertAsync(image);
                                }
                                else
                                {
                                     var image = await _productImageRepository.GetAsync(x => x.Img_id == imageDetailProductId);

                                     image.Img_url = productTTSDetail.Data.Images[0].Url_list[0];
                                     await _productImageRepository.UpdateAsync(image);
                                }

                                CreateImagesIdDto img = new()
                                {
                                    id = imageDetailProductId
                                };

                                imagesId.Add(img);
                        }

                        if (productDB != null)
                        {
                            var channel_data = _shareAppService.ConvertStringToDto<DataChannelProductDto>(productDB.Channel_Data);

                            UpdateProductDto updateProductDto = new()
                            {
                                Product_name = product.Name,
                                Product_status = productTTSDetail.Data.Product_status,
                                Description = productTTSDetail.Data.Description,
                                Category_id = productTTSDetail.Data.Category_list?[^1].id,
                                Brand_id = productTTSDetail.Data.Brand?.Id,
                                Warranty_period = productTTSDetail.Data.Warranty_period != null ? productTTSDetail.Data.Warranty_period.warranty_id : 0,
                                Warranty_policy = productTTSDetail.Data.Warranty_policy,
                                Package_length = channel_data.Package_length,
                                Package_width = channel_data.Package_width,
                                Package_height = channel_data.Package_height,
                                Package_weight = channel_data.Package_weight,
                                Size_chart = channel_data.Size_chart,
                                Product_certifications = channel_data.Product_certifications,
                                Is_cod_open = productTTSDetail.Data.Is_cod_open,
                                Skus = skusProductsDTo,
                                Delivery_service_ids = channel_data.Delivery_service_ids,
                                Images = imagesId,
                            };

                            await UpdateProductDb(updateProductDto, product.Id);
                        }
                        else
                        {
                            DataChannelProductDto dataChannel = new()
                            {
                                Product_name = product.Name,
                                Product_status = productTTSDetail.Data.Product_status,
                                Description = productTTSDetail.Data.Description,
                                Category_id = productTTSDetail.Data.Category_list?[^1].id,
                                Brand_id = productTTSDetail.Data.Brand?.Id,
                                Images = imagesId,
                                Warranty_period = productTTSDetail.Data.Warranty_period != null ? productTTSDetail.Data.Warranty_period.warranty_id : 0,
                                Warranty_policy = productTTSDetail.Data.Warranty_policy,
                                Package_weight = productTTSDetail.Data.Package_weight,
                                Is_cod_open = productTTSDetail.Data.Is_cod_open,
                                Skus = skusProductsDTo
                            };
                            var dataCreate = ShareAppService.ConvertDtoToString(dataChannel);
                            string clientData = "";
                            var produ = _productManager.CreateAsync(channel.Shop_id, product.Id, "", "", 0, dataCreate, clientData, true);
                            await _productRepository.InsertAsync(produ);
                        }
                    }
                }

            }

        }

        [RemoteService(false)]
        public async Task UpdateProductDb(UpdateProductDto updateProductDto, string productId)
        {
            var product = await _productRepository.GetAsync(x => x.Channel_Product_Id == productId);

            DataChannelProductDto dataChannel = new()
            {
                Product_status = updateProductDto.Product_status,
                Product_name = updateProductDto.Product_name,
                Description = updateProductDto.Description,
                Category_id = updateProductDto.Category_id,
                Brand_id = updateProductDto.Brand_id,
                Warranty_period = updateProductDto.Warranty_period,
                Warranty_policy = updateProductDto.Warranty_policy,
                Package_length = updateProductDto.Package_length,
                Package_width = updateProductDto.Package_width,
                Package_height = updateProductDto.Package_height,
                Package_weight = updateProductDto.Package_weight,
                Size_chart = updateProductDto.Size_chart,
                Product_certifications = updateProductDto.Product_certifications,
                Is_cod_open = updateProductDto.Is_cod_open,
                Skus = updateProductDto.Skus,
                Delivery_service_ids = updateProductDto.Delivery_service_ids,
                Images = updateProductDto.Images
            };

            product.Channel_Data = ShareAppService.ConvertDtoToString(dataChannel);

            await _productRepository.UpdateAsync(product);
        }
        [RemoteService(false)]
        // lấy danh sách sản phẩm trong database so sánh với danh sách sản phẩm trong tiktok_shop không có thì xóa
        public async Task DeleteProductRepeat()
        {
            RequestSearchListProductDto searchListProductDto = new()
            {
                page_number = 1,
                page_size = 100
            };
            var listChennal = await _channelAuthenticationAppService.GetListChannelTiktokShop();
            if (listChennal != null)
            {
                foreach (var chennal in listChennal)
                {
                    var productList = _productRepository.GetListAsync(x => x.Shop_Id == chennal.Shop_id);
                    var productListTiktok = await Products(searchListProductDto, chennal.Channel_token);
                    foreach (Product product in productList.Result)
                    {

                        if (!productListTiktok.Data.Products.Any(x => x.Id == product.Channel_Product_Id))
                        {
                            await _productRepository.DeleteAsync(product.Id);
                        }
                    }
                }
            }
        }

        [RemoteService(false)]
        public async Task CheckAndCreateProductAddLinked(List<ProductLinkedListDto> productLinkedListDtos, string channel_token)

        {
            foreach (var product in productLinkedListDtos)
            {
                if (!await _productRepository.AnyAsync(x => x.Channel_Product_Id == product.Id))
                {
                    DataChannelProductDto channelProductDto = new();

                    var productDetail = await GetProductDetail(product.Id, channel_token);

                    channelProductDto.Product_status = productDetail.Data.Product_status;

                    channelProductDto.Product_name = productDetail.Data.Product_name;

                    channelProductDto.Category_id = productDetail.Data.Category_list[0].id;

                    channelProductDto.Description = productDetail.Data.Description;

                    channelProductDto.Package_weight = productDetail.Data.Package_weight;

                    channelProductDto.Is_cod_open = productDetail.Data.Is_cod_open;

                    channelProductDto.Images = ObjectMapper.Map<List<ImageDto>, List<CreateImagesIdDto>>(productDetail.Data.Images);

                    foreach (var sku in productDetail.Data.Skus)
                    {
                        List<SalesAttributesDto> salesAttributes = new();

                        foreach (var sale_att in sku.sales_attributes)
                        {
                            SalesAttributesDto salesAttributesDto = new SalesAttributesDto()
                            {
                                Id = GuidGenerator.Create(),
                                Attribute_id = sale_att.id,
                                Custom_value = sale_att.value_name,
                            };
                            salesAttributes.Add(salesAttributesDto);
                        }

                        CreateSkusProductDto createSkusProductDto = new()
                        {
                            Id = sku.id,
                            Original_price = sku.price.original_price,
                            Sales_attributes = salesAttributes,
                            Seller_sku = sku.seller_sku,
                            Stock_infos = ObjectMapper.Map<List<StockInfosDtoCreateProduct>, List<CreateStockInfosProductDto>>(sku.stock_infos),
                        };
                    }



                    channelProductDto.Skus = ObjectMapper.Map<List<SkusProductDetailDto>, List<CreateSkusProductDto>>(productDetail.Data.Skus);

                    var channel_data = ShareAppService.ConvertDtoToString(channelProductDto);

                    var createProduct = _productManager.CreateAsync(product.Shop_id, product.Id, "", "", EChannel.TiktokShop, channel_data, null, true);

                    await _productRepository.InsertAsync(createProduct);
                }
            }
        }
        /// <summary>
        /// Liên kết shop
        /// </summary>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task ShopLinkedProducts(string channel_token)
        {
            RequestSearchListProductDto searchListProductDto = new()
            {
                page_number = 1,
                page_size = 100
            };
            var channel = await _channelAuthenticationRepository.GetAsync(x => x.Channel_token == channel_token);

            var listProduct = await Products(searchListProductDto, channel.Channel_token);// product in tiktok-shop
            foreach (var product in listProduct.Data.Products)
            {
                var productTTSDetail = await GetProductDetail(product.Id, channel.Channel_token);

                var productDB = await _productRepository.FindAsync(x => x.Channel_Product_Id == product.Id);

                List<CreateSkusProductDto> skusProductsDTo = new();

                if (productTTSDetail.Data.Skus != null)
                {


                    foreach (var sku in productTTSDetail.Data.Skus)
                    {

                        List<CreateStockInfosProductDto> listStock = new();

                        foreach (var stock in sku.stock_infos)
                        {
                            CreateStockInfosProductDto createStockInfosProductDto = new()
                            {
                                Warehouse_id = stock.warehouse_id,
                                Available_stock = stock.available_stock,
                            };
                            listStock.Add(createStockInfosProductDto);
                        }

                        List<SalesAttributesDto> listSaleAtributeDto = new();
                        CreateSkusProductDto skusDto = new()
                        {
                            Id = sku.id,
                            Sales_attributes = listSaleAtributeDto,
                            Seller_sku = sku.seller_sku,
                            Original_price = sku.price?.original_price,
                            Stock_infos = listStock,
                        };
                        if (sku.sales_attributes != null)
                        {
                            foreach (var atribute in sku.sales_attributes)
                            {
                                SalesAttributesDto saleAtributeDto = new()
                                {
                                    Id = GuidGenerator.Create(),
                                    Attribute_id = atribute.id,
                                    Custom_value = atribute.value_name
                                };
                                listSaleAtributeDto.Add(saleAtributeDto);
                            }
                        }

                        skusProductsDTo.Add(skusDto);

                    }
                }

                List<CreateImagesIdDto> imagesId = new();

                if (productTTSDetail.Data.Images != null)
                {
                    for (int i = 0; i < productTTSDetail.Data.Images.Count; i++)
                    {
                        var isAnyImage = await _productImageRepository.AnyAsync(x => x.Img_id == productTTSDetail.Data.Images[i].Id);
                        var imageDetailProductId = productTTSDetail.Data.Images[i].Id;

                        if (!isAnyImage)
                        {
                            foreach (var urlImg in productTTSDetail.Data.Images[i].Url_list)
                            {
                                //
                                var image = _productImageManager.CreateAsync(imageDetailProductId, urlImg, 1);
                                await _productImageRepository.InsertAsync(image);
                            }
                        }
                        else
                        {
                            var image = await _productImageRepository.GetAsync(x => x.Img_id == imageDetailProductId);

                            image.Img_url = productTTSDetail.Data.Images[i].Url_list[0];
                            await _productImageRepository.UpdateAsync(image);
                        }

                        CreateImagesIdDto img = new()
                        {
                            id = imageDetailProductId
                        };

                        imagesId.Add(img);
                    }
                }

                if (productDB != null)
                {
                    var channel_data = _shareAppService.ConvertStringToDto<DataChannelProductDto>(productDB.Channel_Data);

                    UpdateProductDto updateProductDto = new()
                    {
                        Product_name = product.Name,
                        Product_status = productTTSDetail.Data.Product_status,
                        Description = productTTSDetail.Data.Description,
                        Category_id = productTTSDetail.Data.Category_list?[^1].id,
                        Brand_id = productTTSDetail.Data.Brand?.Id,
                        Warranty_period = productTTSDetail.Data.Warranty_period != null ? productTTSDetail.Data.Warranty_period.warranty_id : 0,
                        Warranty_policy = productTTSDetail.Data.Warranty_policy,
                        Package_length = channel_data.Package_length,
                        Package_width = channel_data.Package_width,
                        Package_height = channel_data.Package_height,
                        Package_weight = channel_data.Package_weight,
                        Size_chart = channel_data.Size_chart,
                        Product_certifications = channel_data.Product_certifications,
                        Is_cod_open = productTTSDetail.Data.Is_cod_open,
                        Skus = skusProductsDTo,
                        Delivery_service_ids = channel_data.Delivery_service_ids,
                        Images = imagesId,
                    };

                    await UpdateProductDb(updateProductDto, product.Id);
                }
                else
                {
                    DataChannelProductDto dataChannel = new()
                    {
                        Product_name = product.Name,
                        Product_status = productTTSDetail.Data.Product_status,
                        Description = productTTSDetail.Data.Description,
                        Category_id = productTTSDetail.Data.Category_list?[^1].id,
                        Brand_id = productTTSDetail.Data.Brand?.Id,
                        Images = imagesId,
                        Warranty_period = productTTSDetail.Data.Warranty_period != null ? productTTSDetail.Data.Warranty_period.warranty_id : 0,
                        Warranty_policy = productTTSDetail.Data.Warranty_policy,
                        Package_weight = productTTSDetail.Data.Package_weight,
                        Is_cod_open = productTTSDetail.Data.Is_cod_open,
                        Skus = skusProductsDTo
                    };
                    var dataCreate = ShareAppService.ConvertDtoToString(dataChannel);
                    string clientData = "";
                    var productCreate = _productManager.CreateAsync(channel.Shop_id, product.Id, "", "", 0, dataCreate, clientData, true);
                    await _productRepository.InsertAsync(productCreate);
                }
            }

        }
    }
}
