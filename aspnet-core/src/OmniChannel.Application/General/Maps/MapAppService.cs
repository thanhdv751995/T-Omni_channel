using OmniChannel.Categories;
using OmniChannel.General.Maps;
using OmniChannel.TiktokShop.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.TiktokShop.Maps
{
    [RemoteService(true)]
    public class MapAppService: OmniChannelAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        public MapAppService(ICategoryRepository categoryRepository,
            IAttributeRepository attributeRepository)
        {
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
        }

        public async Task UpdateCategory(List<MapDto> listMapDto, string client_category_id) 
        {
            List<Category> listCategory = new();
            
            foreach (MapDto mapDto in listMapDto)
            {
                var category = await _categoryRepository.GetAsync(mapDto.Id);
                category.Client_category_Id = client_category_id;

                listCategory.Add(category);
            }

            await _categoryRepository.UpdateManyAsync(listCategory);
        }

        public async Task UpdateAttribute(List<MapDto> listMapDto, string client_attribute_id)
        {
            List<Attribute> listAttribute = new();

            foreach (MapDto mapDto in listMapDto)
            {
                var attribute = await _attributeRepository.GetAsync(mapDto.Id);
                attribute.Client_attribute_id = client_attribute_id;

                listAttribute.Add(attribute);
            }

            await _attributeRepository.UpdateManyAsync(listAttribute);
        }
    }
}
