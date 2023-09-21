using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

class EmitLogTopic
{
    public async static Task Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            string[] logLevels = new string[] { "dev.error", "dev.warn", "dev.info", "test.error", "test.warn", "test.info" };

            Random random = new Random(Environment.TickCount);

            int counter = 0;

            while (true)
            {
                string logLevel = logLevels[random.Next(0, logLevels.Length)];

                string message = $"Hello World {counter++} {logLevel} !";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "topic_logs", routingKey: logLevel, basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", logLevel, message);

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
