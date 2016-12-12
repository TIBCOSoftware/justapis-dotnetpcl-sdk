using System.Net;
using System.Net.Security;
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

