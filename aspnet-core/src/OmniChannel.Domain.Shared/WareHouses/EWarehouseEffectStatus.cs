using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OmniChannel.WareHouses
{
    public enum EWarehouseEffectStatus
    {
        [Description("Hiệu quả")]
        EFFECTIVE = 1,
        [Description("Không hiệu quả")]
        NONEFFECTIVE = 2,
        [Description("Hạn chế")]
        RESTRICTED = 3
    }
}
