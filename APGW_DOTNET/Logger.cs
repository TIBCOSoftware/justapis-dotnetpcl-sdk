using System;
using APGW;

namespace APGW_DOTNET
{
	public class Logger : ILogger
	{
		public Logger ()
		{
		}

		public void Log(string message) {
			Console.WriteLine (".net: " + message);
		}

		public void Log(string message, Exception e) {
			Log (message + " : " + e.Message);
		}
	}
}

