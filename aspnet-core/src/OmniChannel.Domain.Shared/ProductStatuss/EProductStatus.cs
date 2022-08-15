using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.ProductStatuss
{
    /// <summary>
    /// trạng thái của sản phẩm tiktokShop
    /// </summary>
    public enum EProductStatus
    {
        draft = 1,
        pending = 2,
        failed = 3,
        live = 4,
        seller_deactivated = 5 ,
        platform_deactivated = 6,
        freeze = 7, 
        deleted = 8
    }
}
