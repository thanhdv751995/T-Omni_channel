using Newtonsoft.Json;
using OmniChannel.Localization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Volo.Abp.AspNetCore.Mvc;

namespace OmniChannel.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OmniChannelController : AbpControllerBase
{
    protected OmniChannelController()
    {
        LocalizationResource = typeof(OmniChannelResource);
    }

    /// <summary>
    /// Tạo nội dung api trả về
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response">Nội dung trả về</param>
    /// <param name="statusCode">Mã trạng thái api trả về</param>
    /// <returns></returns>
    /// <exception cref="HttpResponseException"></exception>
    public T SendResponse<T>(T response, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (statusCode != HttpStatusCode.OK)
        {
            // leave it up to microsoft to make this way more complicated than it needs to be
            // seriously i used to be able to just set the status and leave it at that but nooo... now 
            // i need to throw an exception 
            var badResponse =
                new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
                };

            throw new HttpResponseException(badResponse);
        }
        return response;
    }
}
