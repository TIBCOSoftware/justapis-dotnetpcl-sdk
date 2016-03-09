using System;
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

namespace APGW_DOTNET
{
	[assembly: Xamarin.Forms.Dependency (typeof (IAPRestClient))]
	public class APRestClient : APGW.APRestClient
	{
		public APRestClient ()
		{			
		}
			

	}


}

