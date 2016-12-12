using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public string PostSync(string url="", Dictionary<string,object> body=null)
        {
            return ExecuteSync(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST);
        }

        /// <summary>
        /// Posts A sync.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void PostASync<T>(Callback<T> callback, string url="", Dictionary<string,object> body=null)
        {
            Execute(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST, callback);
        }

        public async void Execute<T>(string url, Dictionary<string,object> body, HTTPMethod method, Callback<T> callback)
        {
            Connect(url, body, method, callback);
        }

        public string ExecuteSync(string url, Dictionary<string,object> body, HTTPMethod method)
        {
            return ConnectSync(url, body, method);
        }           

        public async void Connect<T>(string uri, Dictionary<string,object> body, HTTPMethod method, Callback<T> callback)
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

        public string ConnectSync(string uri, Dictionary<string,object> body, HTTPMethod method)
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

        /// <summary>
        /// Subscribes to a channel.
        /// </summary>
        
        /// <param name="platform">Platform.</param>
        /// <param name="channel">Channel.</param>
        /// <param name="period">Period.</param>
        /// <param name="token">Token.</param>
		/// <param name="name">name object</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async void Subscribe<T>(string url, string platform, string channel, Int64 period, string token,string name, Callback<T> callback) {
            Dictionary<string,object> body = new Dictionary<string,object> ();
            body.Add ("platform", platform);
            body.Add ("channel", channel);
            body.Add ("period", period);
            body.Add ("token", token);
            body.Add("name",name);
            Execute(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST, callback);
        }

        /// <summary>
        /// Unsubscribes the device.
        /// </summary>
        /// <param name="url">method url</param>
        /// <param name="platform">platform string</param>
		/// <param name="platform">environment string</param>
        /// <param name="token">Token.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async void Unsubscribe<T>(string url, string platform,string environment, string channel, string token, Callback<T> callback) {
            Dictionary<string,object> body = new Dictionary<string,object> ();
            body.Add("platform", platform);
            body.Add("environment", environment);
            body.Add ("channel", channel);
            body.Add ("token", token);
            Execute(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST, callback);
        }


        /// <summary>
        /// Publishes to a channel.
        /// </summary>
        /// <param name="channel">Channel.</param>
        /// <param name="environment">Environment.</param>
        /// <param name="payload">Payload.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async void Publish<T>(string url, string channel, string environment, object payload, Callback<T> callback) {
            Dictionary<string,object> body = new Dictionary<string,object> ();
            body.Add ("channel", channel);
            body.Add ("environment", environment);
            body.Add ("payload", payload);
            Execute(Utilities.UpdateUrl(Uri, url), body, HTTPMethod.POST, callback);
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
