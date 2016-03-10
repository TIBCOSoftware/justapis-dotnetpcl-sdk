using System;

namespace APGW
{
	public abstract class ICallback
	{
		Action<Exception> OnError;
	}
}

