using System;
using APGW;

namespace APGW_DOTNET
{
	[assembly: Xamarin.Forms.Dependency (typeof (ILogger))]
	public class Logger : ILogger
	{
		public Logger ()
		{
		}

		public void Log(string message) {
			Console.WriteLine (".net: " + message);
		}
	}
}

