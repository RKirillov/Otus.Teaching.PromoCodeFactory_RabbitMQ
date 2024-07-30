using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Services;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core;
namespace Otus.Teaching.Pcf.ReceivingFromPartner.Integration.Services
{
    public class ProducerMessageService : IProducerMessageService
    {
        private const string Exchange = "Pcf.ReceivingFromPartner.Promocodes";
        private const string RoutingKey = "Pcf.ReceivingFromPartner.Promocode";

        private readonly ConnectionFactory _connectionFactory;
        public ProducerMessageService(IOptions<BusConnectOptions> busOptions)
        {
            _connectionFactory = new()
            {
                UserName = busOptions.Value.Username,
                Password = busOptions.Value.Password,
                HostName = busOptions.Value.Host,
                Port = busOptions.Value.Port,
                VirtualHost = busOptions.Value.VirtualHost
            };

            using var con = _connectionFactory.CreateConnection();
            if (con.IsOpen)
            {
                using var channel = con.CreateModel();
                //объявляю обменник
                channel.ExchangeDeclare(
                    exchange: Exchange,
                    type: ExchangeType.Direct,
                    durable: true);
            }
        }

        public Task PublishMessage<T>(T message)
        {
            using var con = _connectionFactory.CreateConnection();
            if (con.IsOpen)
            {
                using var channel = con.CreateModel();
                var body = JsonSerializer.Serialize(message);
                var bytes = Encoding.UTF8.GetBytes(body);

                channel.BasicPublish(
                    exchange: Exchange,//exchange: _exchangeName, - наименование обменника, может быть несколько
                    routingKey: RoutingKey,//маршрутный ключ
                    body: bytes);//содержимое сообщения
            }
            return Task.CompletedTask;
        }
    }
}
