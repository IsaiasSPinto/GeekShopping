using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using GeekShopping.Email.Messages;
using GeekShopping.Email.Repository;

namespace GeekShopping.Email.MessageConsumer;

public class RabbitMQPaymentConsumer : BackgroundService
{
    private readonly EmailRepository _emailRepository;
    private IConnection _connection;
    private IModel _channel;
    private const string ExchangeName = "DirectPaymentUpdateExchange";
    private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";

    public RabbitMQPaymentConsumer(EmailRepository orderRepository)
    {
        _emailRepository = orderRepository;
        var factory = new ConnectionFactory
        {
            HostName = "127.0.0.1",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct);

        _channel.QueueDeclare(queue: PaymentEmailUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.QueueBind(queue: PaymentEmailUpdateQueueName, exchange: ExchangeName, routingKey: "PaymentEmail");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (channel, e) =>
        {
           var content = Encoding.UTF8.GetString(e.Body.ToArray());
           var message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);

            await ProcessLogs(message);
                        
            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume(PaymentEmailUpdateQueueName, false, consumer);
    }

    private async Task ProcessLogs(UpdatePaymentResultMessage message)
    {
        
        try
        {
            await _emailRepository.LogEmail(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
