using Delivery.API.RabbitMQ.EventProcessing;
using EasyNetQ;
using MessageModels;

namespace Delivery.API.Services;

public class SubscriptionService : BackgroundService
{
    private readonly IBus _bus;
    private readonly MenuEventProcessor _menuEventProcessor;
    private readonly ProductEventProcessor _productEventProcessor;
    private readonly ReservationEventProcessor _reservationEventProcessor;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(IBus bus, MenuEventProcessor menuEventProcessor,
        ProductEventProcessor productEventProcessor, ReservationEventProcessor reservationEventProcessor, ILogger<SubscriptionService> logger)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _menuEventProcessor = menuEventProcessor ?? throw new ArgumentNullException(nameof(menuEventProcessor));
        _productEventProcessor = productEventProcessor ?? throw new ArgumentNullException(nameof(productEventProcessor));
        _reservationEventProcessor = reservationEventProcessor ?? throw new ArgumentNullException(nameof(reservationEventProcessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        try
        {
            await _bus.SendReceive.ReceiveAsync("CoffeeShop.Data", x => x
           .Add<MenuMessage>(message => _menuEventProcessor.ProcessEvent(message))
           .Add<ProductMessage>(message => _productEventProcessor.ProcessEvent(message))
           .Add<ReservationMessage>(message => _reservationEventProcessor.ProcessEvent(message)));
            _logger.LogInformation("Listening for Messages.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not subscribe to queue: {ex.Message}");
        }
    }
}