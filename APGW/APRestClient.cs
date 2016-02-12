using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime;

namespace APGW
{
    public class APRestClient : IAPRestClient
    {
        private HttpMessageHandler handler;
        private HttpClient httpClient;
        private string responseBody = null;

        public APRestClient() { 
        }

        public APRestClient(HttpMessageHandler handler) {
            this.handler = handler;
            httpClient = new HttpClient(handler);
        }

        private async Task<HttpResponseMessage> Post<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.PostAsync(context.Url, null);
            return response;
        }

        private async Task<HttpResponseMessage> Get<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.GetAsync(context.Url);
            if (context.Gateway != null && context.Gateway.ShouldUseCache)
            { 
                // Cache the response keyed by the url
            }
            return response;
        }

        public string ReadResponse()
        {
            return responseBody;
        }

        public Task<HttpResponseMessage> ExecuteRequest<T>(RequestContext<T> request)
        {
            if (request.Method == HTTPMethod.POST || request.Method == HTTPMethod.PUT) {
                // Send a post
                return Post(request);
            }
            else { 
                // Send a get
                return Get(request);
            }
        }
    }
}
