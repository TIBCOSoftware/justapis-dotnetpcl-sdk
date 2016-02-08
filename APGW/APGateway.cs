using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APGW
{
    
    public class APGateway
    {

        public string Uri { get; set; }
        public string Method { get; set; }

        //private IAPRestClient restClient;

        public IAPRestClient RestClient {
            set {
                RestClient = value;
            }
            get {
                if (RestClient == null)
                {
                    RestClient = new APRestClient();
                }

                return RestClient;
            }
        }

        /// <summary>
        /// Sends a get request
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void Get(string url) {
            Execute(HTTPMethod.GET);    
        }

        public void Execute(HTTPMethod method)
        {
            Connect(Uri, method);
        }

        public void Connect(string uri, HTTPMethod method) { 
            StringRequestContext request = new StringRequestContext(method, uri);

            RestClient.ExecuteRequest(request);
        }

        public class Builder {
            public string Uri { get; set; }
            public string Method { get; set; }

            public APGateway Build() {
                APGateway gw = new APGateway();

                gw.Uri = Uri;
                gw.Method = Method;

                return gw;
            }
        }
    }

    
}
