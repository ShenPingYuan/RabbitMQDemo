using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

class EmitLog
{
    public static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            int counter = 0;

            while (true)
            {
                string message = $"Hello World {counter++} !";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
