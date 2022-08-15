using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.MultiTenancy;
using OmniChannel.Shares;
using Volo.Abp;
using OmniChannel.TiktokShop.Attributes;
using OmniChannel.Channels;
using OmniChannel.Categories;
using OmniChannel.ChannelAuthentications;
using OmniChannel.General.Attributes;

namespace OmniChannel.Attributes
{
    [RemoteService(true)]
    public class AttributeAppService : OmniChannelAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly AttributeManager _attributeManager;
        private readonly IAttributeRepository _attributeRepository;
        private readonly string TiktokShopApiGetListAttribute = OmniChannelConsts.TiktokShopAPIGetListAttribute;
        private readonly CategoryAppService _categoryAppService;
        private readonly IChannelAuthenticationRepository _channelAuthenticationRepository;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly IConfiguration _configuration;

        public AttributeAppService(ShareAppService shareAppService,
            AttributeManager attributeManager,
            IAttributeRepository attributeRepository,
            CategoryAppService categoryAppService,
            IChannelAuthenticationRepository channelAuthenticationRepository,
            ChannelAuthenticationAppService channelAuthenticationAppService,
            IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _attributeManager = attributeManager;
            _attributeRepository = attributeRepository;
            _categoryAppService = categoryAppService;
            _channelAuthenticationRepository = channelAuthenticationRepository;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _configuration = configuration;
        }

        public async Task<ResponseDataDto<DataListAttributeDto>> GetListAttributeTiktokShop(string category_id)
        {
            var firstAuthen = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.TiktokShop);

            var requestParameters = ShareAppService.GetRequestParameters($"category_id={category_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListAttribute, _configuration["AppTiktokShopSetting:App_key"], firstAuthen.Access_token, firstAuthen.Shop_id, _configuration["AppTiktokShopSetting:App_secret"], requestParameters);
            HttpResponseMessage response = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());
            return await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<DataListAttributeDto>>(response);
        }

        public async Task CreateAttribute(CreateAttributeDto createAttributeDto)
        {
            var attribute =  _attributeManager.CreateAsync(createAttributeDto.E_channel, createAttributeDto.Attribute_id, createAttributeDto.Name, createAttributeDto.Client_attribute_id);
            await _attributeRepository.InsertAsync(attribute);
        }

        public async Task<List<GAttributeDto>> GetListAttribute(EChannel eChannel, string name)
        {
            if (name == null || name == "null")
            {
                name = "";
            }

            var listAttribute = await _attributeRepository.GetListAsync(x => x.E_Channel == eChannel && x.Name.ToLower().Trim().Contains(name.Trim().ToLower()));

            return ObjectMapper.Map <List<Attribute>, List<GAttributeDto>>(listAttribute);
        }

        [RemoteService(false)]
        public async Task SeedingAttributeByCategoryDataTiktokShop(EChannel eChannel)
        {
            var attributeCount = await _attributeRepository.GetCountAsync(eChannel);

            if (attributeCount == 0)
            {
                List<Attribute> attributes = new();
                var tiktokShopCategoryData = await _categoryAppService.GetListCategoryTiktokShop();

                var tiktokShopCategoryDataList = _categoryAppService.GetTiktokShopCategoryIsLeafDataList(tiktokShopCategoryData, true);
                var tiktokShopCategoryDataCount = tiktokShopCategoryDataList.Count;

                for (var i = 0; i < tiktokShopCategoryDataCount; i++)
                {
                    var listAttribute = await GetListAttributeTiktokShop(tiktokShopCategoryDataList[i].id);

                    var attributeTiktokShopCount = listAttribute.Data.Attributes.Count;

                    for (var j = 0; j < attributeTiktokShopCount; j++)
                    {
                        if(!attributes.Any(x => x.Attribute_id == listAttribute.Data.Attributes[j].Id))
                        {
                            var attribute = _attributeManager.CreateAsync(eChannel, listAttribute.Data.Attributes[j].Id, listAttribute.Data.Attributes[j].Name, "");

                            attributes.Add(attribute);
                        }
                    }
                }

                await _attributeRepository.InsertManyAsync(attributes);
            }
        }
    }
}
