using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace APGW
{
    public class JsonRequestEncoding: IRequestEncoding
    {
        public JsonRequestEncoding ()
        {
        }

        public string Encode(Dictionary<string,object> body) {
            if (body != null) {
                return JsonConvert.SerializeObject (body).ToString ();      
            } else {
                return "";
            }
        }

        public string Encoding() {
            return "application/json";
        }
    }
}

