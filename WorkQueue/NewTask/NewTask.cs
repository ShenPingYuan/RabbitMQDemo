using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

class NewTask
{
    public static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            int counter = 0;

            while (true)
            {
                string message = $"Hello World {counter++} !";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body);
               
                Console.WriteLine(" [x] Sent {0}", message);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
