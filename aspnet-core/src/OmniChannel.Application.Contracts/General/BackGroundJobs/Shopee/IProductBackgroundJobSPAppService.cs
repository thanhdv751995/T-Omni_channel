using OmniChannel.Channels;
using OmniChannel.General.GProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.BackGroundJobs.Shopee
{
    public interface IProductBackgroundJobSPAppService: IApplicationService
    {
        Task CreateGProduct(CreateGProductDto createGProductDto, string channel_token);
        Task UpdateGProduct(CreateGProductDto createGProductDto, string channel_token);
        Task UpdateGProductPrice(string channel_token, EChannel e_channel, string client_product_id, List<GUpdatePriceDto> listSku);
    }
}
