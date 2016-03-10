using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime;
using Autofac;

namespace APGW
{

    public class APRestClient : IAPRestClient
    {
        private HttpClient httpClient;
        private HttpResponseMessage responseBody;

        /// <summary>
        /// 
        /// </summary>
        public APRestClient() {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemHandler"></param>
        public APRestClient(HttpMessageHandler systemHandler)
        {
            httpClient = new HttpClient(systemHandler);
        }

        /// <summary>
        /// Constructor for APRestClient
        /// 
        /// If the systemHandler is null, it'll use the default HttpClientHandler.
        /// 
        /// </summary>
        /// <param name="systemHandler"></param>
        /// <param name="customHandler"></param>
        public APRestClient(HttpMessageHandler systemHandler, HttpMessageHandler customHandler)
        {
            if (systemHandler != null)
            {
                ((DelegatingHandler)customHandler).InnerHandler = systemHandler;
                httpClient = new HttpClient(customHandler);
            }
            else {
                HttpClientHandler baseSystemHandler = new HttpClientHandler();
                ((DelegatingHandler)customHandler).InnerHandler = baseSystemHandler;
                httpClient = new HttpClient(customHandler);
            }
        }

        private async Task<HttpResponseMessage> Post<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.PostAsync(context.Url, null);
            return response;
        }

        private async Task<HttpResponseMessage> Get<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.GetAsync(context.Url);
            responseBody = response;

            return response;
        }

        public TransformedResponseHttpClient ReadResponse()
        {
            return new TransformedResponseHttpClient(responseBody);
        }

        public async Task<IResponse> ExecuteRequest<T>(RequestContext<T> request)
        {
            LogHelper.Log ("Executing request: " + request.Url);
            if (request.Method == HTTPMethod.POST || request.Method == HTTPMethod.PUT) {
                // Send a post
                HttpResponseMessage result = await Post(request);
                LogHelper.Log ("CORE: finished getting response");               

                IResponse response = new HttpClientResponse (result);

                return response;
            }
            else { 
                // Send a get
                HttpResponseMessage result = await Get(request);
                LogHelper.Log ("CORE: finished getting response");

                IResponse response = new HttpClientResponse (result);

                return response;
            }
        }
    }
}
