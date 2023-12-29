using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var done = false;
            var server = new UdpClient(25520);
            var ipe = new IPEndPoint(IPAddress.Any, 25520);
            var data = String.Empty;

            byte[] rec_array;
            try
            {
                while (!done)
                {
                    rec_array = server.Receive(ref ipe);
                    Console.WriteLine("Received a broadcast from {0}", ipe.ToString());
                    data = Encoding.ASCII.GetString(rec_array, 0, rec_array.Length);
                    Console.WriteLine("Received: {0}\r\rn", data);

                    if (data == "WHOBROKER")
                    {
                        byte[] response = Encoding.ASCII.GetBytes($"HEREBROKER '{ipe.Address}'");
                        server.Send(response, response.Length, ipe.Address.ToString(), 25520);
                        await Console.Out.WriteLineAsync($"{ipe.Address}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            server.Close();
        }



        static async Task MqttTest()
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