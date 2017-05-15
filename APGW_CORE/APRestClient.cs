using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace APGW
{

    public class APRestClient : IAPRestClient
    {
        private HttpClient httpClient;

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
            StringContent content = new StringContent (SerializeBodyToJson(context.PostParam), 
                                        Encoding.UTF8, "application/json");
           
            HttpResponseMessage response = await httpClient.PostAsync(context.Url, content);
            return response;
        }

        private async Task<HttpResponseMessage> Get<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.GetAsync(context.Url);
            return response;
        }

        private async Task<HttpResponseMessage> Put<T>(RequestContext<T> context)
        {
            StringContent content = new StringContent (SerializeBodyToJson(context.PostParam), 
                Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PutAsync (context.Url, content);
            return response;
        }

        private async Task<HttpResponseMessage> Delete<T>(RequestContext<T> context)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync (context.Url);
            return response;
        }

        private string SerializeBodyToJson(Dictionary<string,string> body) {
            if (body != null) {
                return JsonConvert.SerializeObject (body).ToString ();      
            } else {
                return "";
            }
        }

        public async Task<IResponse> ExecuteRequest<T>(RequestContext<T> request)
        {
            LogHelper.Log ("Executing request: " + request.Url);

            HttpResponseMessage result = null;
            switch (request.Method) {
            case HTTPMethod.POST:
                result = await Post(request);
                break;            
            case HTTPMethod.PUT:
                result = await Put (request);
                break;
            case HTTPMethod.DELETE:
                result = await Delete (request);
                break;
            case HTTPMethod.GET:
                result = await Get(request);
                break;
            default:
                throw new Exception("Request method not implemented");
            }

            LogHelper.Log ("CORE: finished getting response");
            IResponse response = new HttpClientResponse (result);

            return response;
        }
    }
}
