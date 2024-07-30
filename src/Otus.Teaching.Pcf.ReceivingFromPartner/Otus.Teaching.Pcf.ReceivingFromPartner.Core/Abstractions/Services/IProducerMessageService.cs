using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Services
{
    public interface IProducerMessageService
    {
        Task PublishMessage<T>(T message);
    }
}
