using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using APGW;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Collections.Generic;

namespace Common
{
    public class subscribedEventArgs : MqttMsgSubscribedEventArgs
    {
        public subscribedEventArgs(ushort messageId, byte[] grantedQosLevels) : base(messageId, grantedQosLevels)
        {
        }
    }

    public class unSubscribedEventArgs : MqttMsgUnsubscribedEventArgs
    {
        public unSubscribedEventArgs(ushort messageId) : base(messageId)
        {
        }
    }

    public class publishEventArgs : MqttMsgPublishEventArgs
    {
        public publishEventArgs(string topic, byte[] message, bool dupFlag, byte qosLevel, bool retain) : base(topic, message, dupFlag, qosLevel, retain)
        {
        }
    }

    public class publishedEventArgs : MqttMsgPublishedEventArgs
    {
        public publishedEventArgs(ushort messageId,bool isPublished) : base(messageId, isPublished)
        {

        }
    }

    
    /// <summary>
    /// MQT.
    /// </summary>
    public class MQTT
    {
        private Dictionary<string,Action<EventArgs>> clientEvents = new Dictionary<string,Action<EventArgs>>();
        private MqttClient client;
        //public enum SslProtocols=MqttSslProtocols;
        public bool isConnected()
        {
            if(this.client.Equals(null)){
                return false;
            }else {
                return this.client.IsConnected;
            }
        }
        
        public MQTT(string brokerHostName,int brokerPort, bool secure=false,X509Certificate caCert=null,X509Certificate clientCert=null)
        {

            if (brokerHostName == null)
                throw new ArgumentNullException("brokerHostName", "is required");

           // MqttSslProtocols sslProtocol = MqttSslProtocols.None;
            client = new MqttClient(brokerHostName);
            //,brokerPort,secure,caCert,clientCert,sslProtocol);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.MqttMsgPublished += client_MqttMsgPublished;
            client.MqttMsgSubscribed += client_MqttMsgSubscrbed;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
           

        }

        public void Connect(string clientId = null, string username = null, string password = null, bool willRetain = false, byte willQosLevel = MqttMsgConnect.QOS_LEVEL_AT_MOST_ONCE, bool willFlag = false, string willTopic = null, string willMessage = null, bool cleanSession = true, ushort keepAlivePeriod = 60)
        {
            try
            {
                if (clientId == null)
                {
                    clientId = "dotnetClient";
                }
                this.client.Connect(clientId, username, password, willRetain, willQosLevel, willFlag, willTopic, willMessage, cleanSession, keepAlivePeriod);
            }catch(Exception e){
                Console.WriteLine(e.Message.ToString());
            }
            
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public void Subscribe(string[] topic,Action<EventArgs> onsubscribe=null,Action<EventArgs> onMessageRecieved=null)
        {
            if (onsubscribe != null)
            {
                clientEvents["subscribed"]= onsubscribe;
            }
            if ( onMessageRecieved != null)
            {
                clientEvents["publishRecieved"] = onMessageRecieved;
            }
            client.Subscribe(topic, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }

        private void client_MqttMsgSubscrbed(object sender, MqttMsgSubscribedEventArgs e)
        {
            if (clientEvents.ContainsKey("subscribed"))
            {
                EventArgs args=e;
                clientEvents["subscribed"](args);
            }
            Debug.WriteLine("Subscribed for Id" + e.MessageId);
        }

        public void unSubscribe(string[] topics,Action<EventArgs> onUnSubscribe=null)
        {
            if (onUnSubscribe !=null)
            {
                clientEvents["unsubscribed"] = onUnSubscribe;
            }
            client.Unsubscribe(topics);
        }

        private void client_MqttMsgUnsubscribed(object sender,MqttMsgUnsubscribedEventArgs e)
        {
            if (clientEvents.ContainsKey("unsubscribed"))
            {
                EventArgs args =e;
                clientEvents["unsubscribed"](args);
            }
            Debug.WriteLine(e.MessageId.ToString());
        }


        public void Publish(string topic, string message,Action<EventArgs> onPublished=null)
        {
            if (onPublished != null)
            {
                
                clientEvents["published"]= onPublished;
            }
            client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,false);
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (clientEvents.ContainsKey("publishRecieved"))
            {
                EventArgs args =e;
                clientEvents["publishRecieved"](args);
            }
            Debug.WriteLine(e.Message.ToString());
        }

        private void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (clientEvents.ContainsKey("published"))
            {
                //publishedEventArgs args =(publishedEventArgs)e;
                clientEvents["published"](e);
            }
            Debug.WriteLine(e.MessageId.ToString());
        }

        
    }
}

