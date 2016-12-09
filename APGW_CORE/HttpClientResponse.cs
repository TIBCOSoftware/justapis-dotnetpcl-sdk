using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace APGW
{
    public class HttpClientResponse: IResponse
    {
        private HttpResponseMessage response;

        public HttpClientResponse() {}

        private String requestUri;

        public HttpClientResponse (HttpResponseMessage response)
        {
            this.response = response;
            this.requestUri = response.RequestMessage.RequestUri.ToString();
        }

        public string RequestUri() {
            return requestUri;
        }

        public Task<string> ReadResponseBodyAsString() {
            return response.Content.ReadAsStringAsync ();
        }

        public Dictionary<string, List<string>> Headers() {
            return null;
        }

        public CacheControlOptions CacheControl() {
            if (response.Headers.CacheControl == null || response.Headers.CacheControl.NoCache) {
                return new CacheControlOptions (false);
            } else {
                return new CacheControlOptions (response.Headers.CacheControl.NoCache);
            }
        }
    }
}

