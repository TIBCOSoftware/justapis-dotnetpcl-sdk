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
        public Boolean ShouldUseCache { get; set; }

        private IAPRestClient _restClient;

        public IAPRestClient RestClient {
            set {
                _restClient = value;
            }
            get {
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
        public void Get(string url) {
            Execute(HTTPMethod.GET);    
        }

        public void Execute(HTTPMethod method)
        {
            Connect(Uri, method);
        }

        public string ReadResponse() {
            return RestClient.ReadResponse();
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
