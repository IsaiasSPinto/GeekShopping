
using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using GeekShopping.OrderApi.RabbitMQSender;

namespace GeekShopping.OrderApi.MessageConsumer;

public class RabbitMQMessageConsumer : BackgroundService
{
    private readonly OrderRepository _orderRepository;
    private IConnection _connection;
    private IModel _channel;
    private IRabbitMQMessageSender _rabbitMQMessageSender;

    public RabbitMQMessageConsumer(OrderRepository orderRepository, IRabbitMQMessageSender sender)
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
        _channel.QueueDeclare(queue: "checkoutqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (channel, e) =>
        {
           var content = Encoding.UTF8.GetString(e.Body.ToArray());
           var order = JsonSerializer.Deserialize<CheckoutHeaderDto>(content);

            await ProccessOrder(order);
                        
            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume("checkoutqueue", false, consumer);
    }

    private async Task ProccessOrder(CheckoutHeaderDto order)
    {
        OrderHeader orderHeader = new()
        {
            UserId = order.UserId,
            FristName = order.FristName,
            LastName = order.LastName,         
            Phone = order.Phone,
            Email = order.Email,
            OrderDetails = new List<OrderDatail>(),
            CardNumber = order.CardNumber,
            CVV = order.CVV,
            ExpiryMonthYear = order.ExpireMountYear,
            CartTotalItens = order.CartTotalItens,
            PurchaseAmount = order.PurchaseAmount,
            DiscountTotal = order.DiscountTotal,
            Time = order.Time,
            OrderTime = DateTime.Now,
            PaymentStatus = false,
            CuponCode = order.CuponCode,
        };

        foreach (var item in order.CartDetails)
        {
            OrderDatail orderDatail = new()
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Count = item.Count,
            };

            orderHeader.CartTotalItens += item.Count;

            orderHeader.OrderDetails.Add(orderDatail);
        }

        await _orderRepository.AddOrder(orderHeader);

        PaymentDto paymentDto = new()
        {
            Name = orderHeader.FristName + " " + orderHeader.LastName,
            CardNumber = orderHeader.CardNumber,
            CVV = orderHeader.CVV,
            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            OrderId = orderHeader.Id,
            PurchaseAmount = orderHeader.PurchaseAmount,
            Email = orderHeader.Email
        };

        try
        {
            _rabbitMQMessageSender.SendMessage(paymentDto,"orderPaymentProcessQueue");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
