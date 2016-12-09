using System;
using System.Net;
using System.Threading.Tasks;
using APGW;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
            //LogHelper.Log ("CORE: Pinning certs...");

            client.ClientCertificates.Clear ();

            foreach (var values in CertManager.GetCerts()) {
                X509Certificate cert = new X509Certificate (values.Value);
              
                client.ClientCertificates.Add (cert);
            }
        }

        private async Task<WebResponse> Post(HttpWebRequest client, string url, Dictionary<string,string> body) {
            client.Method = "POST";

            client = WriteDataToRequestStream (body, client);
       
            WebResponse response = await client.GetResponseAsync ();
            return response;
        }

        private async Task<WebResponse> Put(HttpWebRequest client, string url, Dictionary<string,string> body) {
            client.Method = "PUT";

            client = WriteDataToRequestStream (body, client);

            WebResponse response = await client.GetResponseAsync ();
            return response;
        }

        private async Task<WebResponse> Delete(HttpWebRequest client, string url) {
            client.Method = "DELETE";

            WebResponse response = await client.GetResponseAsync ();
            return response;
        }

        private async Task<WebResponse> Get(HttpWebRequest client, string url) {
            client.Method = "GET";

            WebResponse response = await client.GetResponseAsync ();
            return response;
        }

        private HttpWebRequest WriteDataToRequestStream(Dictionary<string,string> body, HttpWebRequest client) {
            var encoder = new APGW.JsonRequestEncoding ();
            byte[] byteArray = Encoding.UTF8.GetBytes (encoder.Encode(body));

            client.ContentType = encoder.Encoding ();
            client.ContentLength = byteArray.Length;
            Stream dataStream = client.GetRequestStream ();
            dataStream.Write (byteArray, 0, byteArray.Length);
            dataStream.Close ();

            return client;            
        }

        private HttpWebRequest Pin<T>(HttpWebRequest client, RequestContext<T> request) {
            if (request.Gateway != null && request.Gateway.ShouldUsePinning ()) {
                PinCerts (client);
            }  

            return client;
        }

        public async Task<IResponse> ExecuteRequest<T>(RequestContext<T> request) {
            //LogHelper.Log ("Executing request with HttpWebRequest: " + request.Url);

            var client = Pin (CreateClient (request.Url), request);

            switch (request.Method) {
            case HTTPMethod.POST:
                return new HttpWebRequestResponse (await Post (client, request.Url, request.PostParam));
            case HTTPMethod.PUT:
                return new HttpWebRequestResponse (await Put (client, request.Url, request.PostParam));
            case HTTPMethod.DELETE:
                return new HttpWebRequestResponse (await Delete (client, request.Url));
            case HTTPMethod.GET:
                return new HttpWebRequestResponse (await Get (client, request.Url));
            default:
                throw new Exception ("Request method not implemented");
            }                
        }
            
    }
}


