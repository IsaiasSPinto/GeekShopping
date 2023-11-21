using GeekShopping.CartApi.Mensages;
using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.CartApi.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private IConnection _connection;
    public void SendMessage(BaseMessage message, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 15672
        };

        _connection = factory.CreateConnection();

        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        byte[] body = GetMessageAsByteArray(message);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    private byte[] GetMessageAsByteArray(BaseMessage message)
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize<CheckoutHeaderDto>((CheckoutHeaderDto)message, option);

        return Encoding.UTF8.GetBytes(json);
    }
}
