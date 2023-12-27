using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) 
            .WithClientId("123456") 
            .Build();

        mqttClient.UseConnectedHandler(e =>
        {
            Console.WriteLine("Conectado ao servidor MQTT!");
        });

        mqttClient.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Desconectado do servidor MQTT!");
        });

        mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            Console.WriteLine($"Mensagem recebida do tópico: {e.ApplicationMessage.Topic}");
            Console.WriteLine($"Conteúdo da mensagem: {System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });

        await mqttClient.ConnectAsync(options);

        Console.WriteLine("GayzinhoSigiloso conectado ao servidor MQTT de Sexo!");
        Console.ReadLine();

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("topic/sigilo") 
            .WithPayload("Quero da ocu") 
            .WithExactlyOnceQoS()
            .WithRetainFlag()
            .Build();

        await mqttClient.PublishAsync(message);

        Console.WriteLine("Mensagem enviada. Pressione Enter para sair.");
        Console.ReadLine();

        await mqttClient.DisconnectAsync();
    }
}
