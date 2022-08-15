using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.DeleteProductsc;
using OmniChannel.General.GProducts;
using OmniChannel.General.LinkedProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task<ResponseCreateProductDto> CreateProductTiktokShop(CreateProductDto createProductDto, string channel_token);
        Task GetListProductRepeat();
        Task UpdateGProduct(CreateGProductDto createGProductDto, string channel_token);
        Task CreateGProduct(CreateGProductDto createGProductDto, string channel_token);
        Task DeleteProduct(List<string> client_product_ids, string channel_token);
        Task DeleteProductRepeat();
        Task UpdateSkuId(Guid product_id);
        Task UpdateGProductPrice(EChannel e_channel, string client_product_id, string channel_token, List<GUpdatePriceDto> listSku);
        Task CheckAndCreateProductAddLinked(List<ProductLinkedListDto> productLinkedListDtos, string channel_token);
        Task ShopLinkedProducts(string channel_token);
        Task UpDateProductStatus(string channel_product_id, string channel_token);
    }
}
