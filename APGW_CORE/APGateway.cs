using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Autofac;
using Autofac.Builder;
using System.Net.Http.Headers;

namespace APGW
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public class APGateway
    {

        public APGateway() {}

        public string Uri { get; set; }
        public string Method { get; set; }

        public CacheEventListener Listener { get; set; }

        public event ChangedEventHandler Changed;

        public static CertManager CertManager { get; set; }

        private bool _useCaching = true;
        public APGateway UseCaching(bool _useCaching) {
            this._useCaching = _useCaching;
            return this;
        }

        private bool _usePinning = false;
        public APGateway UsePinning(bool state) {
            _usePinning = state;
            return this;
        }

        public bool ShouldUsePinning() {
            return _usePinning;
        }

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }


        private IAPRestClient _restClient;
        public IAPRestClient RestClient 
        {
            set
            {
                _restClient = value;

            }
            get
            {
                if (_restClient == null)
                {
                    _restClient = (IAPRestClient)new APRestClient();
                }

                return _restClient;
            }
        }


        /// <summary>
        /// Sends a get request
        /// 
        /// </summary>
        /// <param name="url"></param>
        public string GetSync(string url="")
        {
            return ExecuteSync(Utilities.UpdateUrl(Uri, url), null, HTTPMethod.GET);
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async void GetAsync<T>(Callback<T> callback, string url="") {
            Execute(Utilities.UpdateUrl(Uri, url), null, HTTPMethod.GET, callback);
        }

        /// <summary>
        /// Posts the sync.
        /// </summary>
        /// <param name="url">URL.</param>
        public string PostSync(string url="", Dictionary<string,string> body=null)
        {
            return ExecuteSync(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST);
        }

        /// <summary>
        /// Posts A sync.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void PostASync<T>(Callback<T> callback, string url="", Dictionary<string,string> body=null)
        {
            Execute(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST, callback);
        }

        public async void Execute<T>(string url, Dictionary<string,string> body, HTTPMethod method, Callback<T> callback)
        {
            Connect(Uri, body, method, callback);
        }

        public string ExecuteSync(string url, Dictionary<string,string> body, HTTPMethod method)
        {
            return ConnectSync(Uri, body, method);
        }           

        public async void Connect<T>(string uri, Dictionary<string,string> body, HTTPMethod method, Callback<T> callback)
        {
            var request = callback.CreateRequestContext ();
            request.Method = method;
            request.Url = uri;
            request.Gateway = this;
            request.PostParam = body;

            var response = await RestClient.ExecuteRequest(request);

            var responseBody = await response.ReadResponseBodyAsString();
            #if DEBUG
            LogHelper.Log ("CORE: response body is " + responseBody);
            #endif
            request.ParseResponse(responseBody);

            // Trigger cache listener
            BindListenerAfterReadingResponse (responseBody, response.RequestUri(), response.CacheControl());

            callback.OnSuccess (request.ParseResponse (responseBody).Result);
        }

        public string ConnectSync(string uri, Dictionary<string,string> body, HTTPMethod method)
        {
            StringRequestContext request = new StringRequestContext(method, uri);           
            request.Gateway = this;
            request.PostParam = body;

            if (Listener != null && Listener.InMemoryCache.HasInCache (uri: uri)) {
                #if DEBUG
                LogHelper.Log ("CORE: in cache");
                LogHelper.Log ("CORE: response body from cache is " + Listener.InMemoryCache.GetFromCache (uri: uri));
                #endif
                return Listener.InMemoryCache.GetFromCache (uri: uri);
            } else {
                #if DEBUG
                LogHelper.Log ("CORE: not in cache");
                #endif
                var task = new Task<string>(() => {    
                    var response = RestClient.ExecuteRequest (request).GetAwaiter().GetResult();

                    var str = response.ReadResponseBodyAsString().GetAwaiter().GetResult();

                    #if DEBUG
                    LogHelper.Log ("CORE: response body is " + str);
                    #endif

                    BindListenerAfterReadingResponse (str, response.RequestUri(), response.CacheControl());

                    return str;

                });
                task.RunSynchronously ();

                return task.Result;

            }
        }

        private void BindListenerAfterReadingResponse(string body, string uri, CacheControlOptions cacheControlValue) {
            if (_useCaching) {
                #if DEBUG
                LogHelper.Log ("CORE: notify listener");
                #endif
                OnChanged (new ResponseEventArgs (body, uri, cacheControlValue));
            }
        }

    }


}
