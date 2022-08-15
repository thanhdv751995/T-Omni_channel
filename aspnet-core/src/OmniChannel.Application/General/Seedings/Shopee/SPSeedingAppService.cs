using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.Channels;
using OmniChannel.Shopee.Attributes;
using OmniChannel.Shopee.Categories;
using OmniChannel.TiktokShop.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace OmniChannel.General.Seedings.Shopee
{
    [RemoteService(false)]
    public class SPSeedingAppService : OmniChannelAppService, ISPSeedingAppService
    {
        private readonly long shop_id = 16498519;

        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryManager _categoryManager;
        private readonly CategorySPAppService _categoryAppService;

        private readonly IAttributeRepository _attributeRepository;
        private readonly AttributeManager _attributeManager;
        private readonly AttributeSPAppService _attributeAppService;

        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;

        public SPSeedingAppService(ICategoryRepository categoryRepository,
            CategoryManager categoryManager,
            CategorySPAppService categoryAppService,
            IAttributeRepository attributeRepository,
            AttributeManager attributeManager,
            AttributeSPAppService attributeAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository,
            ChannelAuthenticationAppService channelAuthenticationAppService)
        {
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
            _categoryAppService = categoryAppService;

            _attributeRepository = attributeRepository;
            _attributeManager = attributeManager;
            _attributeAppService = attributeAppService;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _channelAuthenticationAppService = channelAuthenticationAppService;
        }

        #region Seeding
        public async Task SeedingCategory()
        {
            var anyChannelAuthentication = await _channelAuthenticationRepository.AnyAsync();

            if (anyChannelAuthentication)
            {
                var firstChannelAuthentication = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.Shopee);
                var categoryCount = await _categoryRepository.GetCountAsync(EChannel.Shopee);

                if (categoryCount == 0)
                {
                    List<Category> categories = new();

                    var sPCategoryData = await _categoryAppService.GetShopeeCategories(shop_id, firstChannelAuthentication.Access_token);
                    var sPCategoryDataList = sPCategoryData.response.category_list;
                    var sPCategoryDataCount = sPCategoryDataList.Count;

                    for (var i = 0; i < sPCategoryDataCount; i++)
                    {
                        var category = _categoryManager.CreateAsync(
                            EChannel.Shopee,
                            sPCategoryDataList[i].category_id.ToString(),
                            string.Empty,
                            sPCategoryDataList[i].display_category_name,
                            sPCategoryDataList[i].parent_category_id.ToString());

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
                var firstChannelAuthentication = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.Shopee);
                var attributeCount = await _attributeRepository.GetCountAsync(EChannel.Shopee);

                if (attributeCount == 0)
                {
                    List<OmniChannel.TiktokShop.Attributes.Attribute> attributes = new();

                    //lấy category shopee
                    var listSPCategory = await _categoryAppService.GetShopeeCategories(shop_id, firstChannelAuthentication.Access_token);

                    //lấy attribute là con
                    var sPCategoryDataList = listSPCategory.response.category_list.Where(x => x.has_children == false).ToList();
                    var categorySPCount = sPCategoryDataList.Count;

                    for (var i = 0; i < categorySPCount; i++)
                    {
                        //lấy attribute shopee
                        var listSPAttribute = await _attributeAppService.GetSPAttributes(shop_id, 
                            firstChannelAuthentication.Access_token, 
                            sPCategoryDataList[i].category_id.ToString());

                        var attributeTTSCount = listSPAttribute.response.attribute_list.Count;

                        for (var j = 0; j < attributeTTSCount; j++)
                        {
                            if (!attributes.Any(x => x.Attribute_id == listSPAttribute.response.attribute_list[j].attribute_id.ToString()))
                            {
                                var attribute = _attributeManager.CreateAsync(EChannel.Shopee,
                                    listSPAttribute.response.attribute_list[j].attribute_id.ToString(),
                                    listSPAttribute.response.attribute_list[j].display_attribute_name,
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
                var firstChannelAuthentication = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.Shopee);
                List<Category> categories = new();

                var sPCategoryData = await _categoryAppService.GetShopeeCategories(shop_id, firstChannelAuthentication.Access_token);
                var sPCategoryDataList = sPCategoryData.response.category_list;
                var sPCategoryDataCount = sPCategoryDataList.Count;

                for (var i = 0; i < sPCategoryDataCount; i++)
                {
                    var isAnyCategory = await _categoryRepository.AnyAsync(x => x.Category_Id == sPCategoryDataList[i].category_id.ToString()
                        && x.E_Channel == EChannel.Shopee);

                    if (!isAnyCategory)
                    {
                        var category = _categoryManager.CreateAsync(
                        EChannel.Shopee,
                        sPCategoryDataList[i].category_id.ToString(),
                        string.Empty,
                        sPCategoryDataList[i].display_category_name,
                        sPCategoryDataList[i].parent_category_id.ToString());

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
                var firstChannelAuthentication = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.Shopee);
                List<OmniChannel.TiktokShop.Attributes.Attribute> attributes = new();

                //lấy category shopee
                var listSPCategory = await _categoryAppService.GetShopeeCategories(shop_id, firstChannelAuthentication.Access_token);

                //lấy attribute là con
                var sPCategoryDataList = listSPCategory.response.category_list.Where(x => x.has_children == false).ToList();
                var categorySPCount = sPCategoryDataList.Count;

                for (var i = 0; i < categorySPCount; i++)
                {
                    //lấy attribute shopee
                    var listSPAttribute = await _attributeAppService.GetSPAttributes(shop_id, 
                        firstChannelAuthentication.Access_token, 
                        sPCategoryDataList[i].category_id.ToString());

                    var attributeTTSCount = listSPAttribute.response.attribute_list.Count;

                    for (var j = 0; j < attributeTTSCount; j++)
                    {
                        var isAnyAttribute = await _attributeRepository.AnyAsync(x => x.Attribute_id == listSPAttribute.response.attribute_list[j].attribute_id.ToString()
                        && x.E_Channel == EChannel.Shopee);

                        if (!isAnyAttribute)
                        {
                            var attribute = _attributeManager.CreateAsync(EChannel.Shopee,
                                listSPAttribute.response.attribute_list[j].attribute_id.ToString(),
                                listSPAttribute.response.attribute_list[j].display_attribute_name,
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
