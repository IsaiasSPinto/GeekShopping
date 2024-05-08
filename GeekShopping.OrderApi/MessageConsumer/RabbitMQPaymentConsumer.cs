
using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using GeekShopping.OrderApi.RabbitMQSender;

namespace GeekShopping.OrderApi.MessageConsumer;

public class RabbitMQPaymentConsumer : BackgroundService
{
    private readonly OrderRepository _orderRepository;
    private IConnection _connection;
    private IModel _channel;
    private IRabbitMQMessageSender _rabbitMQMessageSender;
    private const string ExchangeName = "DirectPaymentUpdateExchange";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";
    public RabbitMQPaymentConsumer(OrderRepository orderRepository, IRabbitMQMessageSender sender)
    {
        _orderRepository = orderRepository;
        _rabbitMQMessageSender = sender;
        var factory = new ConnectionFactory
        {
            HostName = "127.0.0.1",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

        _channel.QueueDeclare(queue: PaymentOrderUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _channel.QueueBind(queue: PaymentOrderUpdateQueueName, exchange: ExchangeName, routingKey: "PaymentOrder");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (channel, e) =>
        {
           var content = Encoding.UTF8.GetString(e.Body.ToArray());
           var vo = JsonSerializer.Deserialize<UpdatePaymentResultDto>(content);

            await UpdatePaymentStatus(vo);
                        
            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume(PaymentOrderUpdateQueueName, false, consumer);
    }

    private async Task UpdatePaymentStatus(UpdatePaymentResultDto vo)
    {
        
        try
        {
            await _orderRepository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
