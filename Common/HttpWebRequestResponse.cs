using APGW;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class HttpWebRequestResponse: IResponse
    {
        private WebResponse response;
        
        public HttpWebRequestResponse() {}
               
        public HttpWebRequestResponse (WebResponse response)
        {
            this.response = response;
        }

        public string RequestUri() {
            return response.ResponseUri.ToString ();
        }

        public async Task<string> ReadResponseBodyAsString() {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                return sr.ReadToEnd();
            }
        }

        public Dictionary<string, List<string>> Headers() {
            return null;
        }

        public CacheControlOptions CacheControl() {
            return new CacheControlOptions (false);
        }
    }
}

