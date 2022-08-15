using OmniChannel.Attributes;
using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.TiktokShop.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.General.Seedings.TiktokShop
{
    [RemoteService(false)]
    public class TTSSeedingAppService : OmniChannelAppService, ITTSSeedingAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryManager _categoryManager;
        private readonly CategoryAppService _categoryAppService;

        private readonly IAttributeRepository _attributeRepository;
        private readonly AttributeManager _attributeManager;
        private readonly AttributeAppService _attributeAppService;

        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;

        public TTSSeedingAppService(ICategoryRepository categoryRepository,
            CategoryManager categoryManager,
            CategoryAppService categoryAppService,
            IAttributeRepository attributeRepository,
            AttributeManager attributeManager,
            AttributeAppService attributeAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
            _categoryAppService = categoryAppService;

            _attributeRepository = attributeRepository;
            _attributeManager = attributeManager;
            _attributeAppService = attributeAppService;

            _channelAuthenticationRepository = channelAuthenticationRepository;
        }

        #region Seeding

        public async Task SeedingCategory()
        {
            var anyChannelAuthentication = await _channelAuthenticationRepository.AnyAsync();

            if (anyChannelAuthentication)
            {
                var categoryCount = await _categoryRepository.GetCountAsync(EChannel.TiktokShop);

                if (categoryCount == 0)
                {
                    List<Category> categories = new();

                    var tiktokShopCategoryData = await _categoryAppService.GetListCategoryTiktokShop();
                    var tiktokShopCategoryDataList = _categoryAppService.GetTiktokShopCategoryIsLeafDataList(tiktokShopCategoryData, false);
                    var tiktokShopCategoryDataCount = tiktokShopCategoryDataList.Count;

                    for (var i = 0; i < tiktokShopCategoryDataCount; i++)
                    {
                        var category = _categoryManager.CreateAsync(
                            EChannel.TiktokShop,
                            tiktokShopCategoryDataList[i].id,
                            string.Empty,
                            tiktokShopCategoryDataList[i].local_display_name,
                            tiktokShopCategoryDataList[i].parent_id);

                        categories.Add(category);
                    }

                    if (categories.Count > 0)
                    {
                        await _categoryRepository.InsertManyAsync(categories);
                    }
                }
            }
        }

        public async Task SeedingAttribute()
        {
            var anyChannelAuthentication = await _channelAuthenticationRepository.AnyAsync();

            if (anyChannelAuthentication)
            {
                var attributeCount = await _attributeRepository.GetCountAsync(EChannel.TiktokShop);

                if (attributeCount == 0)
                {
                    List<OmniChannel.TiktokShop.Attributes.Attribute> attributes = new();

                    //lấy category tiktok shop
                    var listTTSCategory = await _categoryAppService.GetListCategoryTiktokShop();

                    //lấy attribute là con
                    var tiktokShopCategoryDataList = _categoryAppService.GetTiktokShopCategoryIsLeafDataList(listTTSCategory, true);
                    var categoryTTSCount = tiktokShopCategoryDataList.Count;

                    for (var i = 0; i < categoryTTSCount; i++)
                    {
                        //lấy attribute tiktok shop
                        var listTTSAttribute = await _attributeAppService.GetListAttributeTiktokShop(tiktokShopCategoryDataList[i].id);

                        var attributeTTSCount = listTTSAttribute.Data.Attributes.Count;

                        for (var j = 0; j < attributeTTSCount; j++)
                        {
                            if (!attributes.Any(x => x.Attribute_id == listTTSAttribute.Data.Attributes[j].Id))
                            {
                                var attribute = _attributeManager.CreateAsync(EChannel.TiktokShop,
                                    listTTSAttribute.Data.Attributes[j].Id,
                                    listTTSAttribute.Data.Attributes[j].Name,
                                    string.Empty);

                                attributes.Add(attribute);
                            }
                        }
                    }

                    if (attributes.Count > 0)
                    {
                        await _attributeRepository.InsertManyAsync(attributes);
                    }
                }
            }
        }

        #endregion

        #region Worker

        public async Task WorkerSeedingCategory()
        {
            var anyChannelAuthentication = await _channelAuthenticationRepository.AnyAsync();

            if (anyChannelAuthentication)
            {
                List<Category> categories = new();

                var tiktokShopCategoryData = await _categoryAppService.GetListCategoryTiktokShop();
                var tiktokShopCategoryDataList = _categoryAppService.GetTiktokShopCategoryIsLeafDataList(tiktokShopCategoryData, false);
                var tiktokShopCategoryDataCount = tiktokShopCategoryDataList.Count;

                for (var i = 0; i < tiktokShopCategoryDataCount; i++)
                {
                    var isAnyCategory = await _categoryRepository.AnyAsync(x => x.Category_Id == tiktokShopCategoryDataList[i].id
                    && x.E_Channel == EChannel.TiktokShop);

                    if (!isAnyCategory)
                    {
                        var category = _categoryManager.CreateAsync(
                            EChannel.TiktokShop,
                            tiktokShopCategoryDataList[i].id,
                            string.Empty,
                            tiktokShopCategoryDataList[i].local_display_name,
                            tiktokShopCategoryDataList[i].parent_id);

                        categories.Add(category);
                    }
                }

                if (categories.Count > 0)
                {
                    await _categoryRepository.InsertManyAsync(categories);
                }
            }
        }

        public async Task WorkerSeedingAttribute()
        {
            var anyChannelAuthentication = await _channelAuthenticationRepository.AnyAsync();

            if (anyChannelAuthentication)
            {
                List<OmniChannel.TiktokShop.Attributes.Attribute> attributes = new();

                //lấy category tiktok shop
                var listTTSCategory = await _categoryAppService.GetListCategoryTiktokShop();

                //lấy attribute là con
                var tiktokShopCategoryDataList = _categoryAppService.GetTiktokShopCategoryIsLeafDataList(listTTSCategory, true);
                var categoryTTSCount = tiktokShopCategoryDataList.Count;

                for (var i = 0; i < categoryTTSCount; i++)
                {
                    //lấy attribute tiktok shop
                    var listTTSAttribute = await _attributeAppService.GetListAttributeTiktokShop(tiktokShopCategoryDataList[i].id);

                    var attributeTTSCount = listTTSAttribute.Data.Attributes.Count;

                    for (var j = 0; j < attributeTTSCount; j++)
                    {
                        var isAnyAttribute = await _attributeRepository.AnyAsync(x => x.Attribute_id == listTTSAttribute.Data.Attributes[j].Id
                        && x.E_Channel == EChannel.TiktokShop);

                        if (!isAnyAttribute)
                        {
                            var attribute = _attributeManager.CreateAsync(EChannel.TiktokShop,
                                listTTSAttribute.Data.Attributes[j].Id,
                                listTTSAttribute.Data.Attributes[j].Name,
                                string.Empty);

                            attributes.Add(attribute);
                        }
                    }
                }

                if (attributes.Count > 0)
                {
                    await _attributeRepository.InsertManyAsync(attributes);
                }
            }
        }

        #endregion
    }
}
