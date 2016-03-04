using System;
using Autofac;

namespace APGW
{
	public class LogHelper
	{
		public LogHelper ()
		{
		}

		public static void Log(string message) {
			using (var scope = Config.Container.BeginLifetimeScope())
			{
				var logger = scope.Resolve<ILogger>();
				logger.Log ("core: " + message);
			}
		}

		public static void Log(string message, Exception e) {
			using (var scope = Config.Container.BeginLifetimeScope())
			{
				var logger = scope.Resolve<ILogger>();
				logger.Log ("core: " + message + " : " + e.Message);
			}
		}
	}
}

