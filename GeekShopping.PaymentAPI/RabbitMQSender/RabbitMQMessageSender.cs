﻿using GeekShopping.MessageBus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using GeekShopping.PaymentAPI.Messages;

namespace GeekShopping.PaymentAPI.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private IConnection _connection;
    private const string ExchangeName = "DirectPaymentUpdateExchange";
    private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    public void SendMessage(BaseMessage message)
    {
        if (ConnectionExists())
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);

            channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
            channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);

            channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");
            channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");

            byte[] body = GetMessageAsByteArray(message);

            channel.BasicPublish(exchange: ExchangeName, "PaymentEmail", basicProperties: null, body: body);
            channel.BasicPublish(exchange: ExchangeName, "PaymentOrder", basicProperties: null, body: body);
        }
        
    }

    private byte[] GetMessageAsByteArray(BaseMessage message)
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, option);

        return Encoding.UTF8.GetBytes(json);
    }

    private void CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            _connection = factory.CreateConnection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private bool ConnectionExists()
    {
        if (_connection != null) return true;
        CreateConnection();
        return _connection != null;
    }
}
