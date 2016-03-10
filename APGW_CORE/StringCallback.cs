using System;

namespace APGW
{
	/// <summary>
	/// String callback.
	/// </summary>
	public class StringCallback: Callback<string>
	{
		public override RequestContext<string> CreateRequestContext() {
			return new StringRequestContext();
		}

	}
}

