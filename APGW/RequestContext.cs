using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace APGW
{
    abstract class RequestContext<T>
    {
        private string url { get; set; }
        private Dictionary<string, string> headers { get; set; }
        private bool shouldPinCert { get; set; }
        private Dictionary<string, string> postParam { get; set; }
        private APGateway gateway { get; set; }

        public RequestContext(APGateway gateway)
        {
            this.gateway = gateway;
        }

        abstract protected TransformedResponse<T> parseResponse(ResponseFromRequest responseFromRequest, Exception e);


    }
}
