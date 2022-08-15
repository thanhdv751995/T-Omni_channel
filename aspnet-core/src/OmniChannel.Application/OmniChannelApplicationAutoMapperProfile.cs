using AutoMapper;
using OmniChannel.Authentications;
using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Clients;
using OmniChannel.CreateProducts;
using OmniChannel.General.Attributes;
using OmniChannel.General.Authenticatios;
using OmniChannel.General.ChannelAuthentications;
using OmniChannel.General.GProducts;
using OmniChannel.General.LinkedProducts;
using OmniChannel.General.Shops;
using OmniChannel.Images;
using OmniChannel.ProductDetail;
using OmniChannel.Products;
using OmniChannel.Shopee.Products;
using OmniChannel.Shopee.Products.CreateProductSP;
using OmniChannel.TiktokShop.Attributes;
using OmniChannel.TiktokShop.ChannelAuthentications;
using OmniChannel.TiktokShop.CreateProducts;
using OmniChannel.TiktokShop.ProductDetail;
using OmniChannel.UpdateProducts;
using OmniChannel.Warehouses;

namespace OmniChannel;

public class OmniChannelApplicationAutoMapperProfile : Profile
{
    public OmniChannelApplicationAutoMapperProfile()
    {
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Category, CategoryDto>();
        CreateMap<ChannelAuthentication, ChannelAuthenticationDto>();
        CreateMap<CreateUpdateChannelAuthenticationDto, ChannelAuthentication>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Attribute, GAttributeDto>();
        CreateMap<CreateProductDto, DataChannelProductDto>();
        CreateMap<Product, DataChannelProductDto>();
        CreateMap<UpdateProductDto, DataChannelProductDto>();
        CreateMap<Product, GProductDto>();
        CreateMap<CreateChannelAuthenticationDto, ChannelAuthentication>();
        CreateMap<DetailProductListDto, ProductLinkedListDto>();
        CreateMap<ReponseProductDetailTikTokShopDto, DataChannelProductDto>();
        CreateMap<SkusProductDetailDto, CreateSkusProductDto>();
        CreateMap<ImageDto, CreateImagesIdDto>();
        CreateMap<ProductBaseInfoDto, ResponseCreateProductSPDto>();
        CreateMap<StockInfosDtoCreateProduct, CreateStockInfosProductDto>();
        CreateMap<ChannelAuthentication, GShopDto>().ForMember(s => s.Shop_name, c => c.MapFrom(x => x.Seller_name))
                                                    .ForMember(s => s.Last_connected_time, c => c.MapFrom(x => x.CreationTime))
                                                    .ForMember(s => s.Is_active, c => c.MapFrom(x => x.IsActive));
        CreateMap<GCreateAuthenticationDto, RequestGetAccessTokenDto>();
        CreateMap<DataReponseGetAccessTokenDto, CreateUpdateChannelAuthenticationDto>();
        CreateMap<ChannelAuthentication, ShopDetailDto>().ForMember(s => s.Shop_name, c => c.MapFrom(x => x.Seller_name));
        CreateMap<WarehouseDto, WareHouseShopDto>();
        CreateMap<DataChannelProductDto, CreateGProductDto>();
        CreateMap<CreateSkusProductDto, GSkuDto>().ForMember(gp => gp.Product_price, cp => cp.MapFrom(x => x.Original_price));
        //CreateMap<SynchronizedProductDto, ClientDataDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
