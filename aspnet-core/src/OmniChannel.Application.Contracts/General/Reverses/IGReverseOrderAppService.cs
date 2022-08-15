using OmniChannel.TiktokShop.Reverses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.General.Reverses
{
    public interface IGReverseOrderAppService : IReverseOrderAppService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<dynamic> GetReverseOrder();
    }
}
