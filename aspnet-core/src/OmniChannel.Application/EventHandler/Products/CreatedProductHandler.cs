using OmniChannel.Channels;
using OmniChannel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace OmniChannel.TiktokShop.EventHandler.Products
{
    public class CreatedProductHandler : ILocalEventHandler<EntityCreatedEventData<Product>>,
          ITransientDependency
    {
        private readonly ProductAppService _productAppService;
        public CreatedProductHandler(ProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<Product> eventData)
        {
            var entity = eventData.Entity;

            switch (entity.E_Channel)
            {
                case EChannel.TiktokShop:
                    await _productAppService.UpdateSkuId(entity.Id);
                    break;
            }

        }
    }
}
