using Microsoft.Extensions.Configuration;
using System;
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
using Hangfire;
using OmniChannel.TiktokShop.Categories;
using MongoDB.Driver;
using OmniChannel.Channels;
using OmniChannel.ChannelAuthentications;

namespace OmniChannel.Categories
{
    [RemoteService(true)]
    public class CategoryAppService : OmniChannelAppService, ICategoryAppService
    {
        private readonly ShareAppService _shareAppService;
        private readonly CategoryManager _categoryManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly string TiktokShopApiGetListCategory = OmniChannelConsts.TiktokShopAPIGetListCategory;
        private readonly string TiktokShopAPIGetCategoryRule = OmniChannelConsts.TiktokShopAPIGetCategoryRule;
        private readonly ChannelAuthenticationAppService _channelAuthenticationAppService;
        private readonly IConfiguration _configuration;

        public CategoryAppService(ShareAppService shareAppService,
            ICategoryRepository categoryRepository,
            CategoryManager categoryManager,
            ChannelAuthenticationAppService channelAuthenticationAppService,
            IConfiguration configuration)
        {
            _shareAppService = shareAppService;
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
            _channelAuthenticationAppService = channelAuthenticationAppService;
            _configuration = configuration;
        }

        [RemoteService(false)]
        public async Task<ResponseDataDto<ResponseDataListCategoryDto>> GetListCategoryTiktokShop()
        {
            var firstAuthen = await _channelAuthenticationAppService.GetFirstAuthentication(EChannel.TiktokShop);

            if(firstAuthen == null)
            {
                return null;
            }

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopApiGetListCategory, 
                _configuration["AppTiktokShopSetting:App_key"], 
                firstAuthen.Access_token, 
                firstAuthen.Shop_id, 
                _configuration["AppTiktokShopSetting:App_secret"], 
                new List<string>());

            HttpResponseMessage response = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            var jsonResult = await HttpClientAppService.ConvertFromHttpResponseMessageToJson<ResponseDataDto<ResponseDataListCategoryDto>>(response);

            return jsonResult;
        }

        [RemoteService(false)]
        public async Task<string> GetCategoryRule(string category_id, string channel_token)
        {
            var channel = await _shareAppService.GetAccessTokenShopId(channel_token);

            var requestParameters = ShareAppService.GetRequestParameters($"category_id={category_id}");

            var url = _shareAppService.GetTiktokShopUrl(TiktokShopAPIGetCategoryRule, 
                _configuration["AppTiktokShopSetting:App_key"], 
                channel.Access_token, 
                channel.Shop_id, 
                _configuration["AppTiktokShopSetting:App_secret"], 
                requestParameters);

            HttpResponseMessage response = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object());

            return await response.Content.ReadAsStringAsync();
        }

        public async Task CreateCategory(CategoryDto categoryDto)
        {
            var category =  _categoryManager.CreateAsync(categoryDto.E_channel, 
                categoryDto.Category_id, 
                string.Empty, 
                categoryDto.Display_name, 
                categoryDto.Parent_id);

            await _categoryRepository.InsertAsync(category);
        }

        public async Task<List<CategoryDto>> GetListCategory(EChannel e_channel, string name)
        {
            var listClient = await _categoryRepository.GetListAsync(e_channel, name);

            var result = ObjectMapper.Map<List<Category>, List<CategoryDto>>(listClient);

            return result;
        }

        [RemoteService(false)]
        public List<ResponseDetailCategoryDto> GetTiktokShopCategoryIsLeafDataList(ResponseDataDto<ResponseDataListCategoryDto> tiktokShopCategoryData, bool isLeaf)
        {
            if (isLeaf)
            {
                return tiktokShopCategoryData.Data.category_list.Where(x => x.is_leaf).ToList();
            }
            else
            {
                return tiktokShopCategoryData.Data.category_list;
            }
            
        }
    }
}
