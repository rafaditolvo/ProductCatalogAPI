using RabbitMQ.Client;
using System.Text;

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService()
    {
        var factory = new ConnectionFactory
        {
            //Evitar hardcode de configuraçoes devem ser colocadas em um arquivo de configuraçao
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    //Evitar hardcode de configuraçoes devem ser colocadas em um arquivo de configuraçao
        _channel.QueueDeclare("novos_produtos", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "", routingKey: "novos_produtos", basicProperties: null, body: body);
    }

    //Um exemplo do metodo repetido em varios lugares
    public bool IsRabbitMQConnected()
    {
        return _connection != null && _connection.IsOpen && _channel != null;
    }
}
