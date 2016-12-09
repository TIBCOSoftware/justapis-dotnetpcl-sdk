using System.Collections.Generic;
using Newtonsoft.Json;

namespace APGW
{
    public class JsonRequestEncoding: IRequestEncoding
    {
        public JsonRequestEncoding ()
        {
        }

        public string Encode(Dictionary<string,string> body) {
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

