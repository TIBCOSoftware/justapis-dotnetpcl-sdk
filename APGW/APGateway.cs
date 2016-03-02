using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;

namespace APGW
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public class APGateway
    {

        public string Uri { get; set; }
        public string Method { get; set; }
        public Boolean ShouldUseCache { get; set; }

        public CacheEventListener Listener { get; set; }

        private IAPRestClient _restClient;

        public event ChangedEventHandler Changed;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

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
        public void Get(string url)
        {
            Execute(HTTPMethod.GET);
        }

        public void Execute(HTTPMethod method)
        {
            Connect(Uri, method);
        }

        public string ReadResponse()
        {

            try
            {
                TransformedResponse<HttpResponseMessage> res = RestClient.ReadResponse();
                Task<string> task = Task.Run(() =>
                res.result.Content.ReadAsStringAsync());

                string body = null;
                if (task != null)
                {
                    body = task.Result;

                    // Notify listener
                    OnChanged(new ResponseEventArgs(body, res.result.RequestMessage.RequestUri.ToString(), res.result.Headers.CacheControl));

                }
                return body;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} Exception caught.", e);
            }
            return null;
        }

        public void Connect(string uri, HTTPMethod method)
        {
            StringRequestContext request = new StringRequestContext(method, uri);

            RestClient.ExecuteRequest(request);
        }

        /// <summary>
        /// Builder used to construct a gateway
        /// </summary>
        public class Builder
        {

            private string _uri;
            public Builder Uri(string uri)
            {
                _uri = uri;
                return this;
            }

            private string _method;
            public Builder Method(string method)
            {
                _method = method;
                return this;
            }

            public APGateway Build()
            {
                APGateway gw = new APGateway();

                gw.Uri = _uri;
                gw.Method = _method;

                return gw;
            }
        }
    }


}
