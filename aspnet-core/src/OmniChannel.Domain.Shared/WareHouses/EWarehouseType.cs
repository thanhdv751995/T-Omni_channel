using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OmniChannel.WareHouses
{
    public enum EWarehouseType
    {
        [Description("Kho bán hàng")]
        SALES_WAREHOUSE = 1,
        [Description("Kho trả hàng")]
        RETURN_WAREHOUSE = 2,
        [Description("Kho trả hàng địa phương")]
        LOCAL_RETURN_WAREHOUSE = 3
    }
}
