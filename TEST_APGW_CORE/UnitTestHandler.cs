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

namespace TEST_APGW_CORE
{
    public class UnitTestHandler: BaseUnitTest
    {
		[SetUp]
		public void Setup() {
			SetupDI ();
		}

        [Test]
        public void test_Handler() {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
            builder.Method(HTTPMethod.GET.ToString());
			builder.Uri("http://localhost/api/user/cacheMe/");
            APGateway gw = builder.Build();

            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
			mockHttp.When("http://localhost/api/user/cacheMe/*")
                    .Respond("application/json", "{'name' : 'foobar'}"); // Respond with JSON

            InMemoryCacheHandler cache = new InMemoryCacheHandler();
            
            gw.RestClient = new APRestClient(mockHttp, cache);
			var listener = new CacheEventListener (gw, cache);

            cache.AddListener(listener);

            // Count listener
            Assert.AreEqual(1, cache.countListeners());

			var body = gw.GetSync("/cacheMe");

            Assert.AreEqual("{'name' : 'foobar'}", body);
            Assert.AreEqual(1, cache.Count());

			Assert.AreEqual("{'name' : 'foobar'}", cache.GetFromCache(uri: "http://localhost/api/user/cacheMe/"));

            // Should reduce the number of listeners
            Assert.AreEqual(0, cache.countListeners());

            mockHttp.Flush();

			// Clear out mock
			//mockHttp.Clear ();

			body = gw.GetSync("foo");
			Assert.AreEqual("{'name' : 'foobar'}", body);
        }

		[Test]
		public void test_Handler_Client_Disabled_Caching() {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
			builder.Method(HTTPMethod.GET.ToString());
			builder.Uri("http://localhost/api/user/cacheMe/");
			APGateway gw = builder.Build();

			var mockHttp = new MockHttpMessageHandler();

			// Setup a respond for the user api (including a wildcard in the URL)
			mockHttp.When("http://localhost/api/user/cacheMe/*")
				.Respond("application/json", "{'name' : 'foobar'}"); // Respond with JSON

			InMemoryCacheHandler cache = new InMemoryCacheHandler();

			gw.RestClient = new APRestClient(mockHttp, cache);
			var listener = new CacheEventListener (gw, cache);

			cache.AddListener(listener);

			var body = gw.UseCaching(false).GetSync("/cacheMe");

			Assert.AreEqual("{'name' : 'foobar'}", body);
			Assert.AreEqual(0, cache.Count());

			mockHttp.Flush();
		}


		[Test]
        public void test_NoCache()
        {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
            builder.Method(HTTPMethod.GET.ToString());
            builder.Uri("http://localhost/api/user/");
            APGateway gw = builder.Build();

            var mockHttp = new MockHttpMessageHandler();

            HttpResponseMessage message = new HttpResponseMessage();
            message.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
            message.Headers.CacheControl.NoCache = true;
            message.Content = new StringContent("foo");

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("http://localhost/api/user/*")
                    .Respond(message); // Respond with JSON

            InMemoryCacheHandler cache = new InMemoryCacheHandler();
            gw.RestClient = new APRestClient(mockHttp, cache);

            cache.AddListener(new CacheEventListener(gw, cache));


            // Count listener
            Assert.AreEqual(1, cache.countListeners());

			var body = gw.GetSync("foo");

            Assert.AreEqual("foo", body);
            Assert.AreEqual(0, cache.Count());

            // Should reduce the number of listeners
            Assert.AreEqual(0, cache.countListeners());

            mockHttp.Flush();

        }
    }
}
