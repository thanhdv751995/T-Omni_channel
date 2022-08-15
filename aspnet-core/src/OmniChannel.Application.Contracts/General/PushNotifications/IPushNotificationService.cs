using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.PushNotifications
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushNotificationService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<dynamic> OrderUpdate(TikTokNotificatio1nModel order, EChannel chanel, string channel_token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<dynamic> ReverseOrderUpdate(TikTokNotificatio2nModel order, EChannel chanel, string channel_token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<dynamic> RecipientAddressUpdate(TikTokNotificatio3nModel order, EChannel chanel, string channel_token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<dynamic> PackageUpdate(TikTokNotificatio4nModel order);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<dynamic> ProductAuditResultUpdate(TikTokNotificatio5nModel order);
    }
}
