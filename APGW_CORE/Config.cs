using System;
using Autofac;

namespace APGW
{
	public class Config
	{
		public Config ()
		{
		}

		public static IContainer Container { get; set; }

		public static void RebuildContainer(IContainer container) {
			Container = container;
		}
	}
}

