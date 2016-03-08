﻿using System;
using System.Collections.Generic;

namespace APGW
{
	public class CertManager
	{
		static Dictionary<string,byte[]> certs = new Dictionary<string,byte[]>();


//		public CertManager ()
//		{
//		}

		public static void addCert(string alias, byte[] data) {
			certs.Add (alias, data);
		}

		public static Dictionary<string,byte[]> GetCerts() {
			return certs;
		}

		public static byte[] getCert(string alias) {
			byte[] val;
			certs.TryGetValue (alias, out val);
			return val;
		}


	}
}

