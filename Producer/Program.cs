
using RabbitMQ.Client;
using System.Text;


List<string> cars = new List<string> { "BWM", "Audi", "Tesla", "Mersedes" };
List<string> colors = new List<string> { "red", "white", "black" };

var random = new Random();

Console.WriteLine("RabbitMQ Topic Exchange Publisher");

var counter = 0;
do
{
    int timeToSleep = random.Next(1000, 3000);
    Thread.Sleep(timeToSleep);

    var factory = new ConnectionFactory { HostName = "localhost" };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

    string routingKey = counter % 4 == 0
        ? "Tesla.red.fast.ecological"
        : counter % 5 == 0
               ? "Mersedes.exclusive.expensive.ecological"
               : GenerateRoutingKey();               

    string message = $"Message type '{routingKey}' from Publisher #{counter++}";

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "topic_logs",
                         routingKey: routingKey,
                         basicProperties: null,
                         body: body);

    Console.WriteLine($"Message type '{routingKey}' is sent into Topic Exchange");
}
while (true);


string GenerateRoutingKey()
{
    return $"{cars[random.Next(0, 3)]}.{colors[random.Next(0, 2)]}";
}