using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Send
{
    class Program
    {
        static async Task Main()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.63",
                UserName = "root",
                Password = "tsr@rabbit.mq",
                Port = 48011,
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                int counter = 0;

                while (true)
                {
                    string message = $"Hello World {counter++} !";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", message);

                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
        }
    }
}
