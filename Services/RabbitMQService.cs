using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

// brew services start rabbitmq


public class RabbitMQNotConnectedException : Exception
{
    public RabbitMQNotConnectedException()
    {
    }

    public RabbitMQNotConnectedException(string message) : base(message)
    {
    }

    public RabbitMQNotConnectedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class RabbitMQServiceImpl : IRabbitMQService, IDisposable
{
    private readonly IConnection? _connection;
    private readonly IModel? _channel;

    public RabbitMQServiceImpl()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection?.CreateModel();

            _channel?.QueueDeclare("novos_produtos", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"Erro: Não foi possível alcançar o RabbitMQ. Certifique-se de que o RabbitMQ está em execução. Detalhes: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro desconhecido ao conectar ao RabbitMQ. Detalhes: {ex.Message}");
        }
    }



    public void SendMessage(string message)
    {
        if (IsRabbitMQConnected())
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel!.BasicPublish(exchange: "", routingKey: "novos_produtos", basicProperties: null, body: body);
        }
        else
        {
            Console.WriteLine("Aviso: O RabbitMQ não está conectado. A mensagem não será enviada.");
            throw new RabbitMQNotConnectedException("RabbitMQ is not connected.");
        }
    }

    public bool IsRabbitMQConnected()
    {
        return _connection != null && _connection.IsOpen && _channel != null;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
