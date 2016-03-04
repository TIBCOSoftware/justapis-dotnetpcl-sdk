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
    public class UnitTestGatewaySetup
    {


        [TestMethod]
        public void TestGatewaySetup()
        {
            APGateway.Builder builder = new APGateway.Builder();

            builder.Uri("http://localhost/api/v1");

            APGateway gw = builder.Build();

            Assert.IsNotNull(gw);

            Assert.AreEqual("http://localhost/api/v1", gw.Uri);
        }

        [TestMethod]
        public void TestGet()
        {
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localost/api/user/*")
                    .Respond("application/json", "{'name' : 'foobar'}"); // Respond with JSON

            // Inject the handler or client into your application code
            using (var client = new HttpClient(mockHttp))
            {
                string str = "";
                Task task = Task.Run(async () =>
                {
                    HttpResponseMessage response = await client.GetAsync("http://localost/api/user/1234");
                    str = await response.Content.ReadAsStringAsync();

                    Assert.AreEqual("{'name' : 'foobar'}", str);
                });
                task.Wait();
            }

            mockHttp.Flush();

        }
    }
}
