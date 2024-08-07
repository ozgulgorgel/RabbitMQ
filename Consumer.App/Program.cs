using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

namespace Consumer.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //baglantı oluşturma
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://qqtnrdtf:yRNoGuuL1LEGKLk2JlWdYvNRQCf8LGxy@puffin.rmq2.cloudamqp.com/qqtnrdtf");

            //baglantı aktifleştirme ve kanal acma
            using IConnection connection = factory.CreateConnection();
            var channel = connection.CreateModel();

          
            channel.QueueDeclare(queue: "fanout-queue",durable:true, exclusive: false,autoDelete:false,arguments:null);
            channel.QueueBind("fanout-queue", exchange: "fanout-exchange", routingKey: string.Empty, arguments: null);

            //queue dan mesaj okuma
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(queue: "fanout-queue", false, consumer);
            consumer.Received += (sender, e) =>
            {
                //kuyruğa gelen mesajın işlendiği yerdir

                //e.Body:kuyruktaki mesajın verisini bütünsel olrak getircektir
                //e.Body.AsSpan veya e.Body.ToaARRAY() :Kuyruktaki mesajın byte verisini getirecektir
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
            };
            Console.Read();
        }
    }
}
