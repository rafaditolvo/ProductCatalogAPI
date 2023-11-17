public interface IRabbitMQService
{
    void SendMessage(string message);

    bool IsRabbitMQConnected();
}
