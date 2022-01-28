using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(Message message);
}