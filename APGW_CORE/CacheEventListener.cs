using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APGW
{
    public class CacheEventListener
    {
        private APGateway Gw;
		public InMemoryCacheHandler InMemoryCache {
			get;
			set;
		}

        public CacheEventListener(APGateway gw, InMemoryCacheHandler inMemoryCache) {
            Gw = gw;
            Gw.Changed += new ChangedEventHandler(ResponseConsumed);

            InMemoryCache = inMemoryCache;
        }

        private void ResponseConsumed(object sender, EventArgs e) {
			#if DEBUG
			LogHelper.Log("CORE: Listener notified...");
			#endif
            var responseEventArgs = e as ResponseEventArgs;
            System.Diagnostics.Debug.WriteLine(">>>> consumed" + responseEventArgs.Body);

            CacheControlHeaderValue cacheControlVal = responseEventArgs.CacheControlValue;
            if (cacheControlVal == null || !cacheControlVal.NoCache)
            {
				#if DEBUG
				LogHelper.Log("CORE: Wrote into cache using key: " + responseEventArgs.Uri);
				#endif

				InMemoryCache.PutIntoCache(uri: responseEventArgs.Uri, body: responseEventArgs.Body);
            }

            Detach();
        }

        public void Detach()
        {
            // Detach the event
            Gw.Changed -= new ChangedEventHandler(ResponseConsumed);

            InMemoryCache.RemoveListener(this);
        }
    }

    class ResponseEventArgs : EventArgs {
        private string body;
        private string uri;
        private CacheControlHeaderValue cacheControlValue;

        public ResponseEventArgs(string body, string uri, CacheControlHeaderValue val   ) { 
            this.body = body;
            this.cacheControlValue = val;
            this.uri = uri;
        }

        public string Body
        {
            get { return body; }
            set { this.body = value; }
        }


        public string Uri
        {
            get { return uri; }
            set { this.uri = value; }
        }

        public CacheControlHeaderValue CacheControlValue
        {
            get { return cacheControlValue; }
            set { this.cacheControlValue = value; }
        }
    }
}
