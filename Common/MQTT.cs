using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Security.Cryptography.X509Certificates;
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
    /// MQTT class.
    /// </summary>
    public class MQTT
    {
        private MqttClient client;
        private Dictionary<string, Action<EventArgs>> clientEvents = new Dictionary<string, Action<EventArgs>>();

        public const byte QOS_LEVEL_AT_MOST_ONCE = 0;
        public const byte QOS_LEVEL_AT_LEAST_ONCE = 1;
        public const byte QOS_LEVEL_EXACTLY_ONCE = 2;

        public enum SslProtocols
        {
            None,
            SSLv3,
            TLSv1_0,
            TLSv1_1,
            TLSv1_2
        }

        /// <summary>
        /// This property provides the connection status of the client
        /// </summary>
        /// <returns>true if client connected false otherwise</returns>
        public bool isConnected()
        {
            if(this.client.Equals(null)){
                return false;
            }else {
                return this.client.IsConnected;
            }
        }
        
        /// <summary>
        /// This function creates the MQTT client object
        /// </summary>
        /// <param name="brokerHostName"></param>
        /// <param name="brokerPort">SSL port is mostly 8883</param>
        /// <param name="secure">true for ssl</param>
        /// <param name="caCert"></param>
        /// <param name="clientCert"></param>
        /// <param name="protocol">use enum sslProtocols to provide the type of ssl protocol to use</param>
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
                client.MqttMsgSubscribed += client_MqttMsgSubscribed;
                client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            }catch(SocketException e){
                throw e;
            }
        }

        /// <summary>
        /// This function conencts with the client using the provided client Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="willRetain"></param>
        /// <param name="willQosLevel"></param>
        /// <param name="willFlag"></param>
        /// <param name="willTopic"></param>
        /// <param name="willMessage"></param>
        /// <param name="cleanSession"></param>
        /// <param name="keepAlivePeriod"></param>
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

        /// <summary>
        /// This function disconnects the current client from broker.
        /// </summary>
        public void Disconnect()
        {
            client.Disconnect();
        }

        /// <summary>
        /// This funciton subscribes with the already connected broker with an array of topics
        /// </summary>
        /// <param name="topic">Array of topics. Each topic can have a maximum length of 65535 and minimum lenght of 1 characters</param>
        /// <param name="qosLevels">Array of bytes for each topic.</param>
        /// <param name="onsubscribe">callback function on successful subscription</param>
        /// <param name="onMessageRecieved">callback function called when a message is recieved</param>
        public void Subscribe(string[] topic,byte[] qosLevels,Action<EventArgs> onsubscribe=null,Action<EventArgs> onMessageRecieved=null)
        {
            //if event callback available for subscribe call it when subscibe
            //is successful
            if (onsubscribe != null)
            {
                clientEvents["subscribed"]= onsubscribe;
            }
            //if event callback available for recieve message call it when
            //a message is recieved on the subscribed channel
            if ( onMessageRecieved != null)
            {
                clientEvents["publishRecieved"] = onMessageRecieved;
            }
            client.Subscribe(topic, qosLevels);

        }

        /// <summary>
        /// private function delegate called when the client subscription is successful
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            if (clientEvents.ContainsKey("subscribed"))
            {
                subscribedEventArgs lArgs = new subscribedEventArgs(e);
                clientEvents["subscribed"](lArgs);
            }
        }

        /// <summary>
        /// this functions unsubscribes the client with the provided topics
        /// </summary>
        /// <param name="topics">array of string topics to subscribe from</param>
        /// <param name="onUnSubscribe">callback method</param>
        public void unSubscribe(string[] topics,Action<EventArgs> onUnSubscribe=null)
        {
            if (onUnSubscribe !=null)
            {
                clientEvents["unsubscribed"] = onUnSubscribe;
            }
            client.Unsubscribe(topics);
        }

        /// <summary>
        /// private function delegate called when the client unsubscribes successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_MqttMsgUnsubscribed(object sender,MqttMsgUnsubscribedEventArgs e)
        {
            if (clientEvents.ContainsKey("unsubscribed"))
            {
                unsubscribedEventArgs lArgs = new unsubscribedEventArgs(e);
                clientEvents["unsubscribed"](lArgs);
            }
        }

        /// <summary>
        /// this function publishes a message on the topic 
        /// </summary>
        /// <param name="topic">string topic to publish to</param>
        /// <param name="message">string message to publish</param>
        /// <param name="onPublished">delegate success callback on publish</param>
        public void Publish(string topic, byte[] message, byte qosLevel=QOS_LEVEL_AT_LEAST_ONCE, bool retain=false, Action<EventArgs> onPublished=null)
        {
            if (onPublished != null)
            {
                clientEvents["published"]= onPublished;
            }
            client.Publish(topic, message, qosLevel,retain);
        }

        /// <summary>
        /// this event method is called when a message is recieved successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (clientEvents.ContainsKey("publishRecieved"))
            {
                publishEventArgs lArgs = new publishEventArgs(e);
                clientEvents["publishRecieved"](lArgs);
            }
        }

        /// <summary>
        /// this event method is called when a message is published successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

