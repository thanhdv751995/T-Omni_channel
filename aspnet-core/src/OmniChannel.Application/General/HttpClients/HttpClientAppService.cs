using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using OmniChannel.HttpMethods;
using Volo.Abp;

namespace OmniChannel.HttpClients
{
    [RemoteService(false)]
    public class HttpClientAppService : OmniChannelAppService
    {
        public HttpClientAppService()
        {
        }

        public static async Task<HttpResponseMessage> GetResponseMessage<T>(string url, EHttpMethod method, T requestBody)
        {
            HttpResponseMessage httpResponseMessage = new();

            using (var _httpClient = new HttpClient())
            {
                switch (method)
                {
                    case EHttpMethod.GET:
                        httpResponseMessage = await _httpClient.GetAsync(url);
                        break;
                    case EHttpMethod.POST:
                        httpResponseMessage = await _httpClient.PostAsJsonAsync(url, requestBody);
                        break;
                    case EHttpMethod.PUT:
                        httpResponseMessage = await _httpClient.PutAsJsonAsync(url, requestBody);
                        break;
                    case EHttpMethod.DELETEFORMBODY:
                        HttpRequestMessage request = new()
                        {
                            Content = JsonContent.Create(requestBody),
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(url)
                        };

                        httpResponseMessage = await _httpClient.SendAsync(request);
                        break;
                }
            }

            await HandleErrorHttpResponeMessage(httpResponseMessage);

            return httpResponseMessage;
        }

        public static async Task HandleErrorHttpResponeMessage(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new UserFriendlyException(await httpResponseMessage.Content.ReadAsStringAsync());
            }
        }

        public static async Task<T> ConvertFromHttpResponseMessageToJson<T>(HttpResponseMessage httpResponseMessage)
        {
            return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
