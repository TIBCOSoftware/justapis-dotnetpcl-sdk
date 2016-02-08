using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGW
{
    public class ResponseFromRequest
    {
        public int StatusCode { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public ResponseFromRequest(int statusCode, String data, Dictionary<string, string> headers) {
            this.StatusCode = statusCode;
            this.Data = data;
            this.Headers = headers;
        }
    }
}
