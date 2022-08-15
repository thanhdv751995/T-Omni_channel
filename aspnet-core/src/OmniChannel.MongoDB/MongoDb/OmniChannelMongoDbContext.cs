using MongoDB.Driver;
using OmniChannel.Products;
using OmniChannel.Categories;
using OmniChannel.Orders;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using OmniChannel.Brands;
using OmniChannel.ChannelAuthentications;
using OmniChannel.TiktokShop.Warehouses;
using OmniChannel.TiktokShop.Attributes;
using OmniChannel.TiktokShop.ProductImages;
using OmniChannel.Reverses;

namespace OmniChannel.MongoDB;

[ConnectionStringName("Default")]
public class OmniChannelMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    public IMongoCollection<Product> Products => Collection<Product>();
    //public IMongoCollection<TiktokAttribute> Attributes => Collection<TiktokAttribute>();
    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //builder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});
        modelBuilder.Entity<Category>(b =>
        {
            b.CollectionName = "Category";
        });
        modelBuilder.Entity<Order>(b =>
        {
            b.CollectionName = "Order";
        });
        modelBuilder.Entity<Reverse>(b =>
        {
            b.CollectionName = "Reverse";
        });
        modelBuilder.Entity<Brand>(b =>
        {
            b.CollectionName = "Brand";
        });
        modelBuilder.Entity<ChannelAuthentication>(b =>
        {
            b.CollectionName = "ChannelAuthentication";
        });
        modelBuilder.Entity<Attribute>(b =>
        {
            b.CollectionName = "Attribute";
        });
        modelBuilder.Entity<Product>(b =>
        {
            /* Sharing the same "AbpUsers" collection
             * with the Identity module's IdentityUser class. */
            b.CollectionName = "Product";
        });
        modelBuilder.Entity<ProductImage>(b =>
        {
            /* Sharing the same "AbpUsers" collection
             * with the Identity module's IdentityUser class. */
            b.CollectionName = "ProductImage";
        });
    }
}
