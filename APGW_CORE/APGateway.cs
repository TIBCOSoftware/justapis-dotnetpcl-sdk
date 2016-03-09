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

        private IAPRestClient<TransformedResponse<HttpResponseMessage>> _restClient;

        public event ChangedEventHandler Changed;

		public static CertManager CertManager { get; set; }

		private bool _useCaching = true;
		public APGateway UseCaching(bool _useCaching) {
			this._useCaching = _useCaching;
			return this;
		}


        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

		public IAPRestClient<TransformedResponse<HttpResponseMessage>> RestClient
        {
            set
            {
                _restClient = value;

            }
            get
            {
                if (_restClient == null)
                {
                    _restClient = new APRestClient();
                }

                return _restClient;
            }
        }

        /// <summary>
        /// Sends a get request
        /// 
        /// </summary>
        /// <param name="url"></param>
		public string GetSync(string url)
        {
            var str = ExecuteSync(HTTPMethod.GET);
			return str;
        }

		public async void GetAsync<T>(string url, Callback<T> callback) {
			Execute(HTTPMethod.GET, callback);
		}

		public async void Execute<T>(HTTPMethod method, Callback<T> callback)
        {
			Connect(Uri, method, callback);
        }

		public string ExecuteSync(HTTPMethod method)
		{
			return ConnectSync(Uri, method);
		}			

		public async void Connect<T>(string uri, HTTPMethod method, Callback<T> callback)
        {
			var request = callback.CreateRequestContext ();
			request.Method = method;
			request.Url = uri;

            var response = await RestClient.ExecuteRequest(request);
			var body = await response.Result.Content.ReadAsStringAsync ();
			#if DEBUG
			LogHelper.Log ("CORE: response body is " + body);
			#endif
			request.ParseResponse(body);

			// Trigger cache listener
			BindListenerAfterReadingResponse (body, response.Result.RequestMessage.RequestUri.ToString (), response.Result.Headers.CacheControl);

			callback.OnSuccess (request.ParseResponse (body).Result);
        }

		public string ConnectSync(string uri, HTTPMethod method)
		{
			StringRequestContext request = new StringRequestContext(method, uri);

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
                    var requestTask = RestClient.ExecuteRequest (request).GetAwaiter().GetResult();

                    var str =  requestTask.Result.Content.ReadAsStringAsync ().GetAwaiter().GetResult();

                    #if DEBUG
                    LogHelper.Log ("CORE: response body is " + str);
                    #endif

                    // Trigger cache listener
                    BindListenerAfterReadingResponse (str, requestTask.Result.RequestMessage.RequestUri.ToString (), requestTask.Result.Headers.CacheControl);

                    return str;

                });
				task.RunSynchronously ();

				return task.Result;

			}
		}

		private void BindListenerAfterReadingResponse(string body, string uri, CacheControlHeaderValue cacheControlValue) {
			if (_useCaching) {
				#if DEBUG
				LogHelper.Log ("CORE: notify listener");
				#endif
				OnChanged (new ResponseEventArgs (body, uri, cacheControlValue));
			}
		}

        /// <summary>
        /// Builder used to construct a gateway
        /// </summary>
//        public class Builder<T> where T : APGateway
//        {
//
//            private string _uri;
//            public Builder<T> Uri(string uri)
//            {
//                _uri = uri;
//                return this;
//            }
//
//            private string _method;
//            public Builder<T> Method(string method)
//            {
//                _method = method;
//                return this;
//            }
//
//            public T Build()
//            {
//                T gw = default(T);
//
//
//                gw.Uri = _uri;
//                gw.Method = _method;
//
//                return gw;
//            }
//        }
    }


}
