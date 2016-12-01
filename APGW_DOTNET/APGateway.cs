using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Net.Security;
using System.Linq;
using System.Net.Http;
using APGW;
using Common;

namespace APGW_DOTNET
{
    /// <summary>
    /// AP gateway.
    /// </summary>
    public class APGateway : APGW.APGateway
    {
        public APGateway ()
        {   
            RestClient = new APHttpWebRequestClient ();
        }

        public APGateway UsePinning(bool state) {            
            base.UsePinning (state);

            if (state == true) {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CertPolicy.ValidateServerCertificate);
                ServicePointManager.MaxServicePointIdleTime = 0;
            }
            return this;
        }

    }
}

