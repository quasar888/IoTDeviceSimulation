using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace IoTDeviceSimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to MQTT broker...");

            // Create a new MQTT client
            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();

            // Configure MQTT client options
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId("IoTDevice1")
                .WithTcpServer("broker.hivemq.com", 1883) // Public MQTT broker
                .WithCleanSession()
                .Build();

            // Connect to the broker
            await mqttClient.ConnectAsync(mqttOptions, CancellationToken.None);
            Console.WriteLine("Connected to MQTT broker.");

            // Publish a message to the topic
            string topic = "iot/devices/sensor1";
            string payload = "{\"temperature\": 25.5, \"humidity\": 60}";

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await mqttClient.PublishAsync(message, CancellationToken.None);
            Console.WriteLine($"Message sent to the topic '{topic}': {payload}");

            Console.ReadLine();

            // Disconnect from the broker
            await mqttClient.DisconnectAsync();
            Console.WriteLine("Disconnected from MQTT broker.");
        }
    }
}
