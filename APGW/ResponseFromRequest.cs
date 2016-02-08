using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGW
{
    class ResponseFromRequest
    {
        private int statusCode { get; set; }
        private string data;
        private Dictionary<string, string> headers;

        public ResponseFromRequest(int statusCode, String data, Dictionary<string, string> headers) {
            this.statusCode = statusCode;
            this.data = data;
            this.headers = headers;
        }
    }
}
