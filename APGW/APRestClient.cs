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

        public async Task<HttpResponseMessage> Post(HTTPMethod method, string url, Dictionary<string, string> param)
        {
            HttpResponseMessage response = await httpClient.PostAsync(url, null);
            return response;
        }

        public async Task<HttpResponseMessage> Get(HTTPMethod method, string url) {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                return response;
        }

        public string ReadResponse()
        {
            return responseBody;
        }

        public async void ExecuteRequest<T>(RequestContext<T> request)
        {
            if (request.Method == HTTPMethod.POST || request.Method == HTTPMethod.PUT) {
                // Send a post
                await Post(request.Method, request.Url, request.PostParam);
            }
            else { 
                // Send a get
                await Get(request.Method, request.Url);
            }
        }
    }
}
