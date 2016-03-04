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
    public class UnitTestLRUCache
    {
        [Test]
        public void testLru() {
            LRUCache<string, string> cache = new LRUCache<string, string>();

            cache.Set("first", "foo");
            cache.Set("second", "bar");

            string result;
            cache.GetVal("first", out result);

            Assert.AreEqual("foo", result);

        }
    }
}
