using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using APGW;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;


namespace UnitTestAPGW
{
    [TestClass]
    public class UnitTestRestClient
    {
        [TestMethod]
        public void TestGetRestClient() {
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localost/api/user/*")
                    .Respond("application/json", "{'name' : 'foobar'}"); // Respond with JSON


                string str = "";
                Task t = Task.Run(async () =>
                {
                    APRestClient restClient = new APRestClient(mockHttp);

                    var response = await restClient.Get(HTTPMethod.GET, "http://localost/api/user/v1");
                    str = await response.Content.ReadAsStringAsync();
                });
                t.Wait();
                Assert.AreEqual("{'name' : 'foobar'}", str);
        }
    }
}
