using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Net.Security;
using System.Linq;
using System.Net.Http;
using System.Net;
using APGW;
using System.Threading.Tasks;

namespace APGW_Android
{
    public class APGateway: APGW.APGateway
    {
        public APGateway ()
        {           
        }

        public APGateway UsePinning(bool state) {
            if (state == true) {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CertPolicy.ValidateServerCertificate);
                ServicePointManager.MaxServicePointIdleTime = 0;
            }
            return this;
        }
    }
}

