using OmniChannel.Channels;
using OmniChannel.WareHouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.Enums
{
    [RemoteService(false)]
    public class EnumAppService : OmniChannelAppService
    {
        public EnumAppService()
        {

        }

        public static List<object> GetListEnums<T>() where T : Enum
        {
            List<object> list = new() { };
            var listOfEnums = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            foreach (var enumValue in listOfEnums)
            {
                list.Add(Enum.GetName(typeof(T), enumValue));
            }
            return list;
        }

        public static string GetNameEnum<T>(object value) where T : System.Enum
        {
            return Enum.GetName(typeof(T), value);
        }
        /// <summary>
        /// danh sách sàn thương mại
        /// </summary>
        /// <returns></returns>
        public static List<object> GetListChannelType()
        {
            return GetListEnums<EChannel>();
        }
        /// <summary>
        /// tình trạng hiệu ứng kho
        /// </summary>
        /// <returns></returns>
        public static List<object> GetListWarehouseEffectStatusType()
        {
            return GetListEnums<EWarehouseEffectStatus>();
        }
        /// <summary>
        /// Loai phu kho
        /// </summary>
        /// <returns></returns>
        public static List<object> GetListWarehouseSubType()
        {
            return GetListEnums<EWarehouseSubType>();
        }
        /// <summary>
        /// Loại kho
        /// </summary>
        /// <returns></returns>
        public static List<object> GetListWarehouseType()
        {
            return GetListEnums<EWarehouseType>();
        }
        public static List<ChannelDto> ListChannelEnum()
        {
            List<ChannelDto> Channels = new();
            var listOfEnums = Enum.GetValues(typeof(EChannel)).Cast<EChannel>().ToList();
            foreach (var enumValue in listOfEnums)
            {
                ChannelDto channelDto = new();
                channelDto.Channel_name = Enum.GetName(typeof(EChannel), enumValue);
                channelDto.E_channel = enumValue;
                Channels.Add(channelDto);
            }
            return Channels;
        }
    }
}
