using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APGW;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Autofac;


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

		[Test]
		public void Test_Get_Sync()
		{
			var mockHttp = new MockHttpMessageHandler();

			// Setup a respond for the user api (including a wildcard in the URL)
			mockHttp.When("http://localhost/api/user/*")
				.Respond("application/json", "{'name' : 'foobar2'}"); // Respond with JSON

			APGateway.Builder builder = new APGateway.Builder ();
			builder.Uri ("http://localhost/api/user/foo");

			APGateway gateway = builder.Build ();
			gateway.RestClient = new APRestClient (mockHttp);

			var str = gateway.GetSync("foo");		

			Assert.AreEqual("{'name' : 'foobar2'}", str);

			mockHttp.Flush();
		}

		[Test]
		public void Test_Get_Async()
		{
			var mockHttp = new MockHttpMessageHandler();

			// Setup a respond for the user api (including a wildcard in the URL)
			mockHttp.When("http://localhost/api/user/*")
				.Respond("application/json", "{'name' : 'foobar2'}"); // Respond with JSON

			APGateway.Builder builder = new APGateway.Builder ();
			builder.Uri ("http://localhost/api/user/foo");

			APGateway gateway = builder.Build ();
			gateway.RestClient = new APRestClient (mockHttp);

			// Use a countdown latch for the asynchronous call
			System.Threading.CountdownEvent CountDown = new System.Threading.CountdownEvent(1);
			string result = null;

			gateway.GetAsync ("/foo", new StringCallback () { 
				OnSuccess = (string s) => { 
					Console.WriteLine("result: " + s);

					result = s;
					CountDown.Signal();
			 }
			});

			CountDown.Wait ();

			Assert.AreEqual("{'name' : 'foobar2'}", result);

			mockHttp.Flush();
		}
	}
}

