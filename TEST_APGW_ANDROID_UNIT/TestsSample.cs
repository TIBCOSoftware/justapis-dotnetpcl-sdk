using System;
using NUnit.Framework;
using APGW;


namespace TEST_APGW_ANDROID_UNIT
{
    [TestFixture]
    public class TestsSample
    {
		
        [SetUp]
        public void Setup ()
        {
        }

		
        [TearDown]
        public void Tear ()
        {
        }

        [Test]
        public void Pass ()
        {
            Console.WriteLine ("test1");
            Assert.True (true);
        }

        [Test]
        public void SetupGateway ()
        {
            APGatewayBuilder g = new APGatewayBuilder ();
            g.Uri ("http://localhost");

            APGateway gw = g.Build ();

            Assert.True (true);

        }

        [Test]
        public void Fail ()
        {
            Assert.False (true);
        }

        [Test]
        [Ignore ("another time")]
        public void Ignore ()
        {
            Assert.True (false);
        }

        [Test]
        public void Inconclusive ()
        {
            Assert.Inconclusive ("Inconclusive");
        }
    }
}

