using OmniChannel.CreateProducts;
using OmniChannel.General.LinkedProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.BackgroundJob
{
    public interface IBackgroundJobAppService : IApplicationService
    {
   //     void CreateProductByList(List<ProductLinkedListDto> productLinkedListDtos, string channel_token);
        Task RefreshAccessTokenTiktokShop();
        void CheckChangeProductTiktokShop();
        void ShopLinkedProduct(string channel_token);
    }
}
