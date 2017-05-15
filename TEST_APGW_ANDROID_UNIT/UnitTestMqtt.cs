using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Common;
using NUnitLite.Runner;
using System.Diagnostics;
using System.Threading;
//using System.Security.Cryptography.X509Certificates;

namespace TEST_APGW_ANDROID_UNIT
{
    [TestFixture]
    class UnitTestMqtt
    {
        public UnitTestMqtt()
        {
        }

        [Test]
        public void createMqttClient()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io",1883);

            Assert.IsNotNull(mqtt_client);
        }

        [Test]
        public void connectToBroker()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io",1883);
            mqtt_client.Connect("123445", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
        }

        [Test]
        public void subscribeChannel()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            Assert.IsNotNull(mqtt_client);
            mqtt_client.Connect("123456", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
            mqtt_client.Subscribe(new string[] { "/dotnet_channel4/topic1/" }, new byte[] { MQTT.QOS_LEVEL_EXACTLY_ONCE },(args) =>
            {
                var lArgs =(subscribedEventArgs)args;
                Assert.IsNotNull(lArgs);
                Assert.AreEqual(lArgs.grantedQosLevels[0],MQTT.QOS_LEVEL_EXACTLY_ONCE);
            });

        }

        [Test]
        public void unsubscribeChannel()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            mqtt_client.Connect("123456", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            mqtt_client.unSubscribe(new string[] { "dotnet_channel" },(value)=>
            {
                Assert.IsNotNull(value);
            });
        }

        [Test]
        public void publishMessage()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            mqtt_client.Connect("1234567", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
            mqtt_client.Subscribe(new string[] { "dotnet_channel4/topic1/" }, new byte[] { MQTT.QOS_LEVEL_EXACTLY_ONCE }, (args)=>
            {
                var lArgs = (subscribedEventArgs)args;
                Assert.IsNotNull(lArgs);
                Assert.AreEqual(lArgs.grantedQosLevels[0], MQTT.QOS_LEVEL_EXACTLY_ONCE);

            },(args)=>
            {
                var lArgs = (publishEventArgs)args;
                Assert.AreEqual(Encoding.UTF8.GetString(lArgs.message),"message");

            });
            
            mqtt_client.Publish("dotnet_channel4/topic1/", Encoding.UTF8.GetBytes("message"),MQTT.QOS_LEVEL_AT_LEAST_ONCE,true, (args) =>
            {
                Assert.IsTrue(((publishedEventArgs)args).isPublished);
            });

        }
        
        [Test]
        public void publishSecureMessage()
        {
            
           /* X509Certificate2 ca_cert =new X509Certificate2(@"C:\Users\shahzad\Documents\justapi-dontnet\TEST_CONSOLE\RootCaClientTest.cer");

            X509Certificate2 cert = new X509Certificate2(@"C:\Users\shahzad\Documents\justapi-dontnet\TEST_CONSOLE\TestCert.cer");

            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 8883,true,ca_cert,cert,MQTT.SslProtocols.SSLv3);
            mqtt_client.Connect("1234567", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
            mqtt_client.Subscribe(new string[] { "/dotnet_channel4/topic1/" }, new byte[] { MQTT.QOS_LEVEL_EXACTLY_ONCE }, (args) =>
            {
                var lArgs = (subscribedEventArgs)args;
                Assert.IsNotNull(lArgs);
                Assert.AreEqual(lArgs.grantedQosLevels, MQTT.QOS_LEVEL_EXACTLY_ONCE);

            }, (args) =>
            {
                var lArgs = (publishEventArgs)args;
                Assert.AreEqual(Encoding.UTF8.GetString(lArgs.message), "message");

            });

            mqtt_client.Publish("dotnet_channel4/topic1/", Encoding.UTF8.GetBytes("message"), MQTT.QOS_LEVEL_AT_LEAST_ONCE, false, (args) =>
            {
                Assert.IsTrue(((publishedEventArgs)args).isPublished);
            });*/

        }

    }
}
