using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace APGW
{
    public abstract class RequestContext<T>
    {
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public bool ShouldPinCert { get; set; }
        public Dictionary<string, string> PostParam { get; set; }
        public APGateway Gateway { get; set; }
        public HTTPMethod Method { get; set; }

        public RequestContext(HTTPMethod method, string url) {
            Method = method;
            Url = url;
        }

        public RequestContext(APGateway gateway)
        {
            this.Gateway = gateway;
        }

        public abstract  TransformedResponse<T> ParseResponse(ResponseFromRequest responseFromRequest, Exception e);


    }
}
