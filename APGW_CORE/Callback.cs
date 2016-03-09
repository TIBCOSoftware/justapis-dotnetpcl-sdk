using System;

namespace APGW
{
	public abstract class Callback<T>
	{
		public Callback ()
		{
		}

		public Action<T> OnSuccess;

		public Action<Exception> OnError;


		public abstract RequestContext<T> CreateRequestContext();

	}
}

