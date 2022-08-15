using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.Shopee.Products
{
    public interface IProductSPAppService : IApplicationService
    {
        Task AutomaticProductUpdate();
    }
}
