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
    public class UnitTestEncoder
    {
        public UnitTestEncoder ()
        {
        }

        [Test]
        public void Test_JsonEncoder() {
            Dictionary<string,object> body = new Dictionary<string,object> ();
            body.Add ("foo", "bar");

            IRequestEncoding encoder = new JsonRequestEncoding ();
            string encodedText = encoder.Encode (body);

            Assert.AreEqual ("{\"foo\":\"bar\"}", encodedText);

            encodedText = encoder.Encode (null);

            Assert.AreEqual ("", encodedText);

            Assert.AreEqual ("application/json", encoder.Encoding ());
        }
    }
}

