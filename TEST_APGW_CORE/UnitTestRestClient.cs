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
	public class UnitTestRestClient : BaseUnitTest
    {
		[SetUp]
		public void Setup() {
			SetupDI ();
		}

        [Test]
        public void TestGetRestClient()
        {
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localost/api/user/*")
                    .Respond("application/json", "{'name' : 'foobar'}"); // Respond with JSON

            string str = "";
            Task t = Task.Run(async () =>
            {
                APRestClient restClient = new APRestClient(mockHttp);
                StringRequestContext s = new StringRequestContext(HTTPMethod.GET, "http://localost/api/user/v1");

                var response = restClient.ExecuteRequest(s);
				response.Wait();
				TransformedResponse<HttpResponseMessage> tt =  response.Result;
				str = await tt.Result.Content.ReadAsStringAsync();
            });
            t.Wait();
            Assert.AreEqual("{'name' : 'foobar'}", str);

            mockHttp.Flush();
        }


    }
}
