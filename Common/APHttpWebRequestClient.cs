using System;
using System.Net;
using System.Threading.Tasks;
using APGW;
using System.Security.Cryptography.X509Certificates;

namespace Common
{
    public class APHttpWebRequestClient: IAPRestClient
    {
        /// <summary>
        /// Hold onto the cookie container
        /// </summary>
        private CookieContainer cookieContainer = new CookieContainer();

        public APHttpWebRequestClient ()
        {
        }

        private HttpWebRequest CreateClient(string url) {
            HttpWebRequest client = WebRequest.CreateHttp (url);

            client.CookieContainer = cookieContainer;

            return client;
        }

        public TransformedResponse<WebResponse> ReadResponse() {
            return null;
        }

        private void PinCerts(HttpWebRequest client) {
            LogHelper.Log ("CORE: Pinning certs...");

            client.ClientCertificates.Clear ();

            foreach (var values in CertManager.GetCerts()) {
                X509Certificate cert = new X509Certificate (values.Value);
              
                client.ClientCertificates.Add (cert);
            }
        }

        public async Task<IResponse> ExecuteRequest<T>(RequestContext<T> request) {
            LogHelper.Log ("Executing request with HttpWebRequest: " + request.Url);

            var client = CreateClient (request.Url);

            if (request.Gateway != null && request.Gateway.ShouldUsePinning ()) {
                PinCerts (client);
            }

            if (request.Method == HTTPMethod.POST || request.Method == HTTPMethod.PUT) {
                // Send a post
                client.Method = "POST";

                LogHelper.Log ("CORE: finished getting response");

                WebResponse response = await client.GetResponseAsync ();

                return new HttpWebRequestResponse (response);
            } else { 
                // Send a get
                client.Method = "GET";

                LogHelper.Log ("CORE: finished getting response");

                WebResponse response = await client.GetResponseAsync ();

                return new HttpWebRequestResponse (response);
            }   
        }
            
    }
}


