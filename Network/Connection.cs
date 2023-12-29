using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace Network
{
    public class Connection
    {
        public IMqttClient MqttClient { get; set; } = null!;

        public Connection()
        {
            // накрутить логирование
            MqttClient = new MqttFactory().CreateMqttClient();
            

            //MqttClient.ConnectAsync()

        }
    }
}