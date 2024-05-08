using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMQSender;
using GeekShopping.PaymentProcessor;

namespace GeekShopping.PaymentAPI.MessageConsumer;

public class RabbitMQMessageConsumer : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private readonly IProcessPayment _processPayment;
    private readonly IRabbitMQMessageSender _messageSender;

    public RabbitMQMessageConsumer(IProcessPayment process, IRabbitMQMessageSender sender)
    {
        _processPayment = process;
        _messageSender = sender;
        var factory = new ConnectionFactory
        {
            HostName = "127.0.0.1",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "orderPaymentProcessQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (channel, e) =>
        {
           var content = Encoding.UTF8.GetString(e.Body.ToArray());
           var paymentMessage = JsonSerializer.Deserialize<PaymentMessage>(content);

            await ProccessPayment(paymentMessage);
                        
            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume("orderPaymentProcessQueue", false, consumer);
    }

    private async Task ProccessPayment(PaymentMessage payment)
    {
        var result =  _processPayment.PaymentProcessor();

        UpdatePaymentResultMessage paymenteResult = new()
        {
            Status = result,
            Email = payment.Email,
            OrderId = payment.OrderId
        };
        try
        {
            _messageSender.SendMessage(paymenteResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
