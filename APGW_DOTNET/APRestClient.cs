using System;
using System.Net.Http;
using System.Net;


namespace APGW_DOTNET
{
	public class APRestClient : APGW.APRestClient
	{
		public APRestClient ()
		{
		}

		public void PinCert() {
			

			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://YourServer/sample.asp");
		}
	}
}

