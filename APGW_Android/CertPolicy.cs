using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Net;
using APGW;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Net.Security;
using System.Linq;

namespace APGW_Android
{
    /// <summary>
    /// Cert policy.
    /// </summary>
    public class CertPolicy : ICertificatePolicy

    {
        public CertPolicy ()
        {
        }

        /// <summary>
        /// Checks the validation result.
        /// </summary>
        /// <returns><c>true</c>, if validation result was checked, <c>false</c> otherwise.</returns>
        /// <param name="srvPoint">Srv point.</param>
        /// <param name="certificate">Certificate.</param>
        /// <param name="request">Request.</param>
        /// <param name="certificateProblem">Certificate problem.</param>
        public bool CheckValidationResult(ServicePoint srvPoint, 
            X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            // You can do your own certificate checking.
            // You can obtain the error values from WinError.h.

            // Return true so that any certificate will work with this sample.
            return true;
        }

        /// <summary>
        /// Validates the server certificate.
        /// </summary>
        /// <returns><c>true</c>, if server certificate was validated, <c>false</c> otherwise.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="certificate">Certificate.</param>
        /// <param name="chain">Chain.</param>
        /// <param name="policyErrors">Policy errors.</param>
        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors policyErrors)
        {
            // Logic to determine the validity of the certificate
            if (policyErrors == SslPolicyErrors.None) {
                return true;
            }

            if (policyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
            {
                Console.WriteLine("Certificate chain error: {0}", policyErrors);
                foreach (var chainstat in chain.ChainStatus)
                {
                    Console.WriteLine("{0}", chainstat.Status);
                    Console.WriteLine("{0}", chainstat.StatusInformation);
                }

                foreach (var cert in CertManager.GetCerts()) {
                    X509Certificate c = new X509Certificate (cert.Value);

                    if (c.GetCertHashString().Equals(certificate.GetCertHashString())) {
                        return true;
                    }
                }
                return false;
            }

            Console.WriteLine("Certificate error: {0}", policyErrors);

            return false;
        }
    }
}

