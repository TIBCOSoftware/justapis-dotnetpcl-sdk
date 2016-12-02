using System;
using System.Collections.Generic;
using NUnit.Framework;
using APGW;
using Newtonsoft.Json.Linq;

namespace TEST_APGW_CORE
{
    [TestFixture]
    class UnitTestPubSub : BaseUnitTest
    {

        public UnitTestPubSub()
        {
        }

        [SetUp]
        public void Setup()
        {
            SetupDI();
        }

        [Test]
        public void Test_Subscribe()
        {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
            builder.Uri("https://ceaseless-trains-4183.staging.nanoscaleapi.io");
            
            APGateway gw = builder.Build();
            Assert.IsNotNull(gw);
            
            gw.Subscribe("push/pushRemoteEndpoint/subscribe", "apns", "dotnet_chnl_sub", 3153600, "dotnet_token","test_name", callback: new StringCallback()
            {
                OnSuccess = (string s) => {
                    Assert.IsEmpty(s);
                    Console.WriteLine("result: " + s);
                }
            });
            
        }


        [Test]
        public void Test_UnSubscribe()
        {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
            builder.Uri("https://ceaseless-trains-4183.staging.nanoscaleapi.io/push/pushRemoteEndpoint");

            APGateway gw = builder.Build();
            Assert.IsNotNull(gw);

            gw.Unsubscribe("unsubscribe","apns", "development", "dotnet_chnl_sub", "dotnet_token", callback: new StringCallback()
            {
                OnSuccess = (string s) => {
                    Assert.IsEmpty(s);
                    Console.WriteLine("result: " + s);
                }
            });

        }

        [Test]
        public void Test_Publish()
        {
            APGatewayBuilder<APGateway> builder = new APGatewayBuilder<APGateway>();
            builder.Uri("https://ceaseless-trains-4183.staging.nanoscaleapi.io");

            APGateway gw = builder.Build();
            Assert.IsNotNull(gw);

            gw.Subscribe("push/pushRemoteEndpoint/subscribe", "apns", "dotnet_chnl_sub", 3153600, "dotnet_token","test_name", callback: new StringCallback()
            {
                OnSuccess = (string s) => {
                    Assert.IsEmpty(s);
                    Console.WriteLine("result: " + s);
                }
            });

            object jsonObj = JObject.Parse(@"{ 'apns': { 'aps': { 'message':'pub sub dot net message','alert':'default','badge':1} } }");
        
            gw.Publish("push/pushRemoteEndpoint/publish", "dotnet_chnl_sub", "development", jsonObj, callback: new StringCallback()
            {
                OnSuccess = (string s) => {
                    Assert.IsEmpty(s);
                    Console.WriteLine("result: " + s);
                }
            });

        }
    }
}
