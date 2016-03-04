using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using APGW;


namespace TEST_APGW_CORE 
{
	
	public class UnitTestAPGW : BaseUnitTest
	{
		
		public UnitTestAPGW ()
		{


		}

		[SetUp]
		public void Setup() {
			SetupDI ();
		}

		[Test]
		public void TestMe() {
			APGateway.Builder builder = new APGateway.Builder ();
			builder.Uri ("http://localhost");

			APGateway gw = builder.Build ();

			Assert.IsNotNull (gw);
			Assert.AreEqual ("http://localhost", gw.Uri);
			
		}
	}
}

