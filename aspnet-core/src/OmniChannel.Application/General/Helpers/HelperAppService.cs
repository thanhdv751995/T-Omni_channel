using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.General.Helpers
{
    public class HelperAppService : OmniChannelAppService
    {
        public HelperAppService()
        {

        }

        public T ConvertObjToDto<T>(object obj)
        {
            return ObjectMapper.Map<object, T>(obj);
        }

        public object ConvertDtoToObj<T>(T dynamicDto)
        {
            return ObjectMapper.Map<T, object>(dynamicDto);
        }

        public static string ConvertObjToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ConvertDtoToString<T>(T dynamicDto)
        {
            return JsonConvert.SerializeObject(dynamicDto);
        }

        public static object ConvertStringToObj(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public T ConvertStringToDto<T>(string value)
        {
            return ConvertObjToDto<T>(ConvertStringToObj(value));
        }
    }
}
