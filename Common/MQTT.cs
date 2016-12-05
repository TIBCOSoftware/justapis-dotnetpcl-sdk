using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using APGW;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Common
{   

    public class subscribeEventArgs :EventArgs
    {
        private MqttMsgSubscribeEventArgs args;

        public ushort messageId { get { return args.MessageId; } }
        public byte[] qosLevel { get { return args.QoSLevels; } }
        public string[] topics { get { return args.Topics; } }
        public subscribeEventArgs(MqttMsgSubscribeEventArgs args)
        {
            this.args = args;
        }
    }

    public class subscribedEventArgs : EventArgs
    {
        private MqttMsgSubscribedEventArgs args;

        public ushort messageId { get { return args.MessageId; } }
        public byte[] grantedQosLevels { get { return args.GrantedQoSLevels; } }
        public subscribedEventArgs(MqttMsgSubscribedEventArgs args)
        {
            this.args = args;
        }
    }


    public class unsubscribEventArgs : EventArgs
    {
        private MqttMsgUnsubscribeEventArgs args;
        public ushort messageId { get{ return args.MessageId;}}
        public string[] topics{ get{return args.Topics;}}

        public unsubscribEventArgs(MqttMsgUnsubscribeEventArgs args) 
        {
            this.args = args;
        }
    }

    public class unsubscribedEventArgs : EventArgs
    {
        private MqttMsgUnsubscribedEventArgs args;
        public ushort messageId{ get{ return args.MessageId;}}

        public unsubscribedEventArgs(MqttMsgUnsubscribedEventArgs args) 
        {
            this.args = args;
        }
    }

    public class publishEventArgs : EventArgs
    {
        private MqttMsgPublishEventArgs args;
        public byte[] message{ get{return args.Message;}}
        public byte qosLevel{ get{return args.QosLevel;}}
        public bool retain{ get{return args.Retain;}}
        public string topic{ get{return args.Topic;}}

        public publishEventArgs(MqttMsgPublishEventArgs args)
        {
            this.args = args;
        }
    }

    public class publishedEventArgs : EventArgs
    {
        private MqttMsgPublishedEventArgs args;
        public ushort messageId{ get{ return args.MessageId;}}
        public bool isPublished { get { return args.IsPublished;}}

        public publishedEventArgs(MqttMsgPublishedEventArgs args)
        {
            this.args = args;
        }
    }

    /// <summary>
    /// MQT.
    /// </summary>
    public class MQTT
    {
        private Dictionary<string,Action<EventArgs>> clientEvents = new Dictionary<string,Action<EventArgs>>();
        private MqttClient client;
       
        public enum SslProtocols
        {
            None,
            SSLv3,
            TLSv1_0,
            TLSv1_1,
            TLSv1_2
        }
        public bool isConnected()
        {
            if(this.client.Equals(null)){
                return false;
            }else {
                return this.client.IsConnected;
            }
        }
        
        public MQTT(string brokerHostName,int brokerPort=1883, bool secure=false,X509Certificate caCert=null,X509Certificate clientCert=null,SslProtocols protocol=SslProtocols.None)
        {

            if (brokerHostName == null)
                throw new ArgumentNullException(nameof(brokerHostName), " is required");
            try
            {
                MqttSslProtocols sslProtocol = (MqttSslProtocols)((int)protocol);
                client = new MqttClient(brokerHostName,brokerPort,secure,caCert,clientCert,sslProtocol);
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                client.MqttMsgPublished += client_MqttMsgPublished;
                client.MqttMsgSubscribed += client_MqttMsgSubscrbed;
                client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            }catch(SocketException e){
                throw e;
            }



        }

        public void Connect(string clientId, string username = null, string password = null, bool willRetain = false, byte willQosLevel = MqttMsgConnect.QOS_LEVEL_AT_MOST_ONCE, bool willFlag = false, string willTopic = null, string willMessage = null, bool cleanSession = true, ushort keepAlivePeriod = 60)
        {
            try
            {
                if (clientId == null)
                {
                    throw new ArgumentNullException(nameof(clientId), " is required");
                }
                this.client.Connect(clientId, username, password, willRetain, willQosLevel, willFlag, willTopic, willMessage, cleanSession, keepAlivePeriod);
            }catch(Exception e){
                throw e;
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
                subscribedEventArgs lArgs = new subscribedEventArgs(e);
                clientEvents["subscribed"](lArgs);
            }
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
                unsubscribedEventArgs lArgs = new unsubscribedEventArgs(e);
                clientEvents["unsubscribed"](lArgs);
            }
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
                publishEventArgs lArgs = new publishEventArgs(e);
                clientEvents["publishRecieved"](lArgs);
            }
        }

        private void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (clientEvents.ContainsKey("published"))
            {
                publishedEventArgs lArgs =new publishedEventArgs(e);
                clientEvents["published"](lArgs);
            }
        }

        
    }
}

