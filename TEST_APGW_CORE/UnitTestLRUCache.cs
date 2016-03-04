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
    public class UnitTestLRUCache
    {
        [TestMethod]
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
