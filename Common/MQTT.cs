using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using APGW;
using System.Net;
using System.Text;

namespace Common
{
    /// <summary>
    /// MQT.
    /// </summary>
    public class MQTT
    {

        private MqttClient client;

        public MQTT (string destination)
        {
            client = new MqttClient(IPAddress.Parse(destination)); 

            // Subscribe to MqttMsgPublishReceived event
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
        }

        public void Connect() {
            string clientId = Guid.NewGuid().ToString(); 
            client.Connect(clientId); 
        }

        public void Subscribe(string topic) {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        public void Publish(string topic, string message) {
            client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); 
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
        { 
            LogHelper (e.Message.ToString);
        } 
    }
}

