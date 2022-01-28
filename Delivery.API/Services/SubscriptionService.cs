using Delivery.API.RabbitMQ.EventProcessing;
using EasyNetQ;
using MessageModels;

namespace Delivery.API.Services;

public class SubscriptionService : BackgroundService
{
    private readonly IBus _bus;
    private readonly IEventProcessor _eventProcessor;

    public SubscriptionService(IBus bus, IEventProcessor eventProcessor)
    {
        _bus = bus;
        _eventProcessor = eventProcessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        await _bus.SendReceive.ReceiveAsync("CoffeeShop.Data", x => x.Add<Message>(message => _eventProcessor.ProcessEvent(message)));
        Console.WriteLine("--> Listening for Messages");
    }
}