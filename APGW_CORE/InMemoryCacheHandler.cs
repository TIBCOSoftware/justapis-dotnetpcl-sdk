using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace APGW
{
    public class InMemoryCacheHandler : DelegatingHandler, ICacheManager
    {
        private ConcurrentDictionary<string, string> inMemoryCache;


        private List<CacheEventListener> listeners = new List<CacheEventListener>();

        public List<CacheEventListener> Listeners
        {
            get { return listeners; }
        }


        public InMemoryCacheHandler() {
            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;

            int initialCapacity = 101;

            inMemoryCache = new ConcurrentDictionary<string, string>(concurrencyLevel, initialCapacity);
        }

        public void AddListener(CacheEventListener listener) {
            listeners.Add(listener);
        }

        public void RemoveListener(CacheEventListener listener) {
            listeners.Remove(listener);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
           
            return response;
        }

        public int Count() {
            return inMemoryCache.Count;
        }

		public string GetFromCache(String uri, string requestMethod  = "GET") {
            string val;
            inMemoryCache.TryGetValue(uri, out val);
            return val;
        }

		public void PutIntoCache(String uri, String body, string requestMethod = "GET") {
            inMemoryCache.TryAdd(uri, body);
        }

		public bool HasInCache(string uri, string requestMethod  = "GET") {
			return inMemoryCache.ContainsKey (uri);
		}

		public void ClearCache() {
			inMemoryCache.Clear ();
		}

        public int countListeners() {
            return Listeners.Count();
        }
            
    }
}
