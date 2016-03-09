using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime;
using Xamarin.Forms;
using Autofac;

namespace APGW
{

	public class APRestClient : IAPRestClient<TransformedResponse<HttpResponseMessage>>
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

        public TransformedResponse<HttpResponseMessage> ReadResponse()
        {
            return new TransformedResponse<HttpResponseMessage>(responseBody);
        }

		public async Task<TransformedResponse<HttpResponseMessage>> ExecuteRequest<T>(RequestContext<T> request)
        {
			LogHelper.Log ("Executing request: " + request.Url);
            if (request.Method == HTTPMethod.POST || request.Method == HTTPMethod.PUT) {
                // Send a post
				HttpResponseMessage result = await Post(request);
				LogHelper.Log ("@@ finished getting response");

				return new TransformedResponse<HttpResponseMessage> (result);
            }
            else { 
                // Send a get
				HttpResponseMessage result = await Get(request);
				LogHelper.Log ("@@ finished getting response");

				return new TransformedResponse<HttpResponseMessage> (result);
            }
        }
    }
}
