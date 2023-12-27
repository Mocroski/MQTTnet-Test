using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

class Program
{
    static async Task Main(string[] args)
    {
        var options = new MqttServerOptionsBuilder()
            .WithConnectionBacklog(100)
            .WithDefaultEndpointPort(1883)
            .Build();

        var mqttServer = new MqttFactory().CreateMqttServer();

        mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
        {
            Console.WriteLine($"Gay conectado: {e.ClientId}");
        });

        mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e =>
        {
            Console.WriteLine($"Gay desconectado: {e.ClientId}");
        });

        mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
        {
            Console.WriteLine($"Mensagem recebida no tópico: {e.ApplicationMessage.Topic}");
            Console.WriteLine($"Conteúdo da mensagem: {System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });

        await mqttServer.StartAsync(options);

        Console.WriteLine("Servidor MQTT iniciado. Pressione Enter para encerrar.");
        Console.ReadLine();

        await mqttServer.StopAsync();
    }
}
