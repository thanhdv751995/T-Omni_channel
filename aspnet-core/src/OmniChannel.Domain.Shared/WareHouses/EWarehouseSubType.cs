using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OmniChannel.WareHouses
{
    public enum EWarehouseSubType
    {
        [Description("Kho hàng nội địa")]
        DOMESTIC_WAREHOUSE = 1,
        [Description("Kho hàng xuyên lục địa")]
        CB_OVERSEA_WAREHOUSE = 2,
        [Description("Kho vận chuyển trực tiếp")]
        CB_DIRECT_SHIPPING_WAREHOUSE = 3
    }
}
