using System;

namespace TEST_APGW_IOS_UNIT
{
	public class Logger : APGW.ILogger
	{
		public Logger ()
		{
		}

		public void Log(string message) {
			Console.WriteLine ("test: " + message);
		}

		public void Log(string message, Exception e) {
			Log (message + " : " + e.Message);
		}
			
	}
}

