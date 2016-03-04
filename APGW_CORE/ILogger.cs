using System;

namespace APGW
{
	public interface ILogger
	{
		void Log(string message);

		void Log(string message, Exception e);
	}
}

