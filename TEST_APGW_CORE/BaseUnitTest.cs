using System;
using Autofac;
using APGW;

namespace TEST_APGW_CORE
{
	public class BaseUnitTest
	{
		public BaseUnitTest ()
		{
		}

		public void SetupDI() {
			var builder = new ContainerBuilder();
			builder.RegisterType<Logger> ().As<ILogger> ();
			Config.RebuildContainer (builder.Build());	
		}
	}
}

