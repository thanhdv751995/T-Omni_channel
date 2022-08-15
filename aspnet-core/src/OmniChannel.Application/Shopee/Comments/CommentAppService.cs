using OmniChannel.HttpClients;
using OmniChannel.HttpMethods;
using OmniChannel.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.Shopee.Comments
{
    public class CommentAppService : OmniChannelAppService
    {
        private readonly string API_GET_COMMENT = ShopeeConst.API_GET_COMMENT;

        private readonly ShareAppService _shareAppService;

        public CommentAppService(ShareAppService shareAppService)
        {
            _shareAppService = shareAppService;
        }

        public async Task<string> GetComment(long shop_id, string access_token)
        {
            var url = _shareAppService.GetShopeeUrl(API_GET_COMMENT, shop_id, access_token, new List<string>() { });

            var httpResponseMessage = await HttpClientAppService.GetResponseMessage(url, EHttpMethod.GET, new object() { });

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
