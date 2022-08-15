using OmniChannel.Channels;
using OmniChannel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.RecurringJobs.TiktokShop
{
    public class TTSRecurringJobAppSerivce : OmniChannelAppService, ITTSRecurringJobAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductAppService _productAppService;
        public TTSRecurringJobAppSerivce(IProductRepository productRepository,
            ProductAppService productAppService)
        {
            _productRepository = productRepository;
            _productAppService = productAppService;
        }

        [RemoteService(false)]
        public async Task UpdateSkuIdIfNull(double days)
        {
            var currentTime = DateTime.UtcNow.AddDays(days);

            var listProduct = await _productRepository.GetListAsync(x => x.E_Channel == EChannel.TiktokShop
            && currentTime <= x.CreationTime);

            foreach (var product in listProduct)
            {
                await _productAppService.UpdateSkuId(product.Id);
            }
        }
    }
}
