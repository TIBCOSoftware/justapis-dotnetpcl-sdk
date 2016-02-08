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

        /// <summary>
        /// Sends a get request
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void get(string url) { 
        
        }

        class Builder {
            string Uri { get; set; }
            string Method { get; set; }

            public APGateway build() {
                APGateway gw = new APGateway();

                gw.Uri = Uri;
                gw.Method = Method;

                return gw;
            }
        }
    }

    
}
