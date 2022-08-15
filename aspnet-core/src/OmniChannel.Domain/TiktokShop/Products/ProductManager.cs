using OmniChannel.Channels;
using OmniChannel.CreateProducts;
using OmniChannel.TiktokShop.CreateProducts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.Products
{
    public class ProductManager : DomainService
    {
        public Product CreateAsync(
                 string shop_id,
                 string channel_product_id,
                 string client_product_id,
                 string client_category_id,
                 EChannel e_Channel,
                 string channel_data,
                 string client_data,
                 bool isActive
         )
        {
            return new Product(
                GuidGenerator.Create(),
                shop_id,
                channel_product_id,
                client_product_id,
                client_category_id,
                e_Channel,
                channel_data,
                client_data,
                isActive,
                !client_product_id.IsNullOrWhiteSpace()
            );
        }
    }
}
