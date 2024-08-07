using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace Publisher.App
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://qqtnrdtf:yRNoGuuL1LEGKLk2JlWdYvNRQCf8LGxy@puffin.rmq2.cloudamqp.com/qqtnrdtf");
            //baglantıyı aktifleştirme ve kanal acma
            using IConnection connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicReturn += (sender, e) =>
            {
                var messagee = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"message ilgili  kuyruğa gönderilemedi.{messagee}");

            };

            //fanout exchange
            channel.ExchangeDeclare("fanout-exchange", ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);


            for (int i = 0; i < 11; i++)
            {
                var message = $"message {i}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "fanout-exchange", routingKey:string.Empty,mandatory:true,null,messageBody);
            }
           

            Console.WriteLine("message gönderildi");

            Console.Read();
        }
    }
}
