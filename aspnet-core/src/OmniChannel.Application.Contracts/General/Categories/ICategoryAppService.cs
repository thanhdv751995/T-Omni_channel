using OmniChannel.Categories;
using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.TiktokShop.Categories
{
    public interface ICategoryAppService: IApplicationService
    {
        Task CreateCategory(CategoryDto categoryDto);
    }
}
