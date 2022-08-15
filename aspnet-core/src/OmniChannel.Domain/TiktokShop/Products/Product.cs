using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.TiktokShop.CreateProducts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.Products
{
    public class Product : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        public string Shop_Id { get; set; }
        public string Channel_Product_Id { get; set; }
        public string Client_Product_Id { get; set; }
        public string Client_Category_Id { get; set; }
        public EChannel E_Channel { get; set; }
        public string Channel_Data { get; set; }
        public string Client_Data { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsLinked { get; set; }
        private Product()
        {
        }
        internal Product(
                 Guid id,                
                 string shop_id,
                 string channel_product_id,
                 string client_product_id,
                 string client_category_id,
                 [NotNull] EChannel e_channel,
                 string channel_data,
                 string client_data,
                 bool isActive,
                 bool isLinked
                )
                : base(id)
        {
            Shop_Id = shop_id;
            Channel_Product_Id = channel_product_id;
            Client_Product_Id = client_product_id;
            Client_Category_Id = client_category_id;
            E_Channel = e_channel;
            Channel_Data = channel_data;
            Client_Data = client_data;
            IsActive = isActive;
            IsLinked = isLinked;
        }
    }
}
