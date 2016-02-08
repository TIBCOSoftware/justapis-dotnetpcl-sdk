using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using APGW;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Threading.Tasks;


namespace UnitTestAPGW
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGatewaySetup()
        {
            APGateway.Builder builder = new APGateway.Builder();
            builder.Uri = "http://localhost/api/v1";
            APGateway gw = builder.Build();

            Assert.IsNotNull(gw);

            Assert.AreEqual("http://localhost/api/v1", gw.Uri);
        }

        public void TestGet() { 
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localost/api/user/*")
                    .Respond("application/json", "{'name' : 'Test McGee'}"); // Respond with JSON

            // Inject the handler or client into your application code
            var client = new HttpClient(mockHttp);
                Task task = new Task(() => {
                var response = client.GetAsync("http://localost/api/user/1234");
                // or without async: var response = client.GetAsync("http://localost/api/user/1234").Result;
            });
            
		    task.Start();
		    task.Wait();

        }
    }
}
