using Microsoft.AspNetCore.Mvc;
using OmniChannel.General.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.Controllers
{
    [Route("api/enum")]
    public class EnumController : OmniChannelController
    {

        public EnumController()
        {
        }
        /// <summary>
        /// danh sách kênh
        /// </summary>
        /// <returns></returns>
        [HttpGet("E-channel")]
        public List<object> ListChannelType()
        {
            return EnumAppService.GetListChannelType();
        }
        [HttpGet("Warehouse-Effect-Status-Type")]
        public  List<object> ListWarehouseEffectStatusType()
        {
            return EnumAppService.GetListWarehouseEffectStatusType();
        }
        [HttpGet("Warehouse-Sub-Type")]
        public  List<object> ListWarehouseSubType()
        {
            return EnumAppService.GetListWarehouseSubType();
        }
        [HttpGet("Warehouse-Type")]
        public  List<object> ListWarehouseType()
        {
            return EnumAppService.GetListWarehouseType();
        }
        [HttpGet("Channel-enum")]
        public List<ChannelDto> ListChannelEnum()
        {
            return EnumAppService.ListChannelEnum();
        }
    }
}
