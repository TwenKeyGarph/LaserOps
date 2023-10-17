using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string topic = "laserops";

            MqttFactory factory = new MqttFactory();

            IMqttClient mqttClient = factory.CreateMqttClient();

            MqttClientOptions options = new MqttClientOptionsBuilder()
                .WithTcpServer("uburtu", 1883)
                .WithClientId(Guid.NewGuid().ToString())
                .WithCleanSession()
                .Build();

            MqttClientConnectResult connectResult = await mqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine("Connected to MQTT broker successfully.");

                await mqttClient.SubscribeAsync(topic);

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                    return Task.CompletedTask;
                };

                for (int i = 0; i < 10; i++)
                {
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload($"Hello, MQTT! Message number {i}")
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithRetainFlag()
                        .Build();
                    Console.ReadLine();
                    await mqttClient.PublishAsync(message);
                    await Task.Delay(1000); 
                }

                await mqttClient.UnsubscribeAsync(topic);
                await mqttClient.DisconnectAsync();
            }
            else
            {
                Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
            }
        }
    }
}