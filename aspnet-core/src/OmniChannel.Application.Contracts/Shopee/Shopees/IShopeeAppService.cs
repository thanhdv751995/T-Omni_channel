using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.Shopee.Shopees
{
    public interface IShopeeAppService
    {
        Task RefreshAccessToken();
    }
}
