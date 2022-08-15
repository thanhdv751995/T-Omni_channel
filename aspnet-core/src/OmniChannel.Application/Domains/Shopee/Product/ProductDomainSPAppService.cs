using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.General.GProducts;
using OmniChannel.Products;
using OmniChannel.Shares;
using OmniChannel.Shopee.Products;
using OmniChannel.Shopee.Products.CreateProductSP;
using OmniChannel.TiktokShop.CreateProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.Domains.Shopee.Product
{
    [RemoteService(false)]
    public class ProductDomainSPAppService : OmniChannelAppService
    {
        private readonly ProductManager _productManager;
        private readonly IProductRepository _productRepository;

        private readonly ShareAppService _shareAppService;

        public ProductDomainSPAppService(ProductManager productManager,
            IProductRepository productRepository,
            ShareAppService shareAppService)
        {
            _productManager = productManager;
            _productRepository = productRepository;
            _shareAppService = shareAppService;
        }

        public async Task CreateProductIntoDatabase(ResponseCreateProductSPDto responseCreateProductSPDto,
            string client_data,
            EChannel eChannel,
            string client_product_id,
            string shop_id,
            string category_id)
        {
            var channel_data = ShareAppService.ConvertDtoToString(responseCreateProductSPDto);

            var product = _productManager.CreateAsync(shop_id,
                responseCreateProductSPDto.item_id.ToString(),
                client_product_id,
                category_id,
                eChannel,
                channel_data,
                client_data,
                true);

            await _productRepository.InsertAsync(product);
        }

        public async Task UpdateProductDatbase(CreateGProductDto createGProductDto, 
            long category_id, 
            ImageProductInfoBaseDto images, 
            Products.Product product)
        {
            var channel_data = _shareAppService.ConvertStringToDto<ResponseCreateProductSPDto>(product.Channel_Data);

            channel_data.weight = float.Parse(createGProductDto.Package_weight);
            channel_data.description = createGProductDto.Description;
            channel_data.category_id = category_id;
            channel_data.item_name = createGProductDto.Product_name;
            channel_data.images = images;

            product.Channel_Data = ShareAppService.ConvertDtoToString(channel_data);
            product.Client_Data = createGProductDto.Client_data;

            await _productRepository.UpdateAsync(product);
        }
    }
}
