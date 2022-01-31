using AutoMapper;
using Delivery.API.Models;
using Delivery.API.Services;
using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Processes message based on Event written inside message
    /// </summary>
    /// <param name="message"></param>
    public void ProcessEvent(Message message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.Add:
                Add(message);
                break;
            case EventType.Update:
                Update(message);
                break;
            case EventType.Delete:
                Delete(message);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Determines Type of the event
    /// </summary>
    /// <param name="platformPublishedDto"></param>
    /// <returns></returns>
    private EventType DetermineEvent(Message message)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = _mapper.Map<GenericEvent>(message);
        switch (eventType.Event)
        {
            case "Add":
                Console.WriteLine("--> Add Event Determined");
                return EventType.Add;
            case "Update":
                Console.WriteLine("--> Update Event Determined");
                return EventType.Update;
            case "Delete":
                Console.WriteLine("--> Delete Event Determined");
                return EventType.Delete;
            default:
                Console.WriteLine("-->Could Not Determine Event");
                return EventType.Undetermined;
        }
    }


    /// <summary>
    /// Adds object to DB based on message type
    /// </summary>
    /// <param name="message">contains menu, product or reservation object with specified event</param>
    private async void Add(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepository = scope.ServiceProvider.GetRequiredService<MenuService>();
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepository.CreateAsync(menu);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add to DB {ex.Message}");
                }
            }
            else if (message is ProductMessage)
            {
                try
                {
                    var productRepository = scope.ServiceProvider.GetRequiredService<ProductService>();
                    var product = _mapper.Map<Product>(message);
                    await productRepository.CreateAsync(product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add to DB {ex.Message}");
                }
            }
            else
            {
                try
                {
                    var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepository.CreateAsync(reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add to DB {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Updates record in DB based on message type
    /// </summary>
    /// <param name="message">contains menu, product or reservation object with specified event</param>
    private async void Update(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepository = scope.ServiceProvider.GetRequiredService<MenuService>();
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepository.UpdateAsync(menu.Id, menu);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update DB {ex.Message}");
                }
            }
            else if (message is ProductMessage)
            {
                try
                {
                    var productRepository = scope.ServiceProvider.GetRequiredService<ProductService>();
                    var product = _mapper.Map<Product>(message);
                    await productRepository.UpdateAsync(product.Id, product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update DB {ex.Message}");
                }
            }
            else
            {
                try
                {
                    var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepository.UpdateAsync(reservation.Id, reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update DB {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Removes record from DB based on message type
    /// </summary>
    /// <param name="message"></param>
    /// <returns>contains menu, product or reservation object with specified event</returns>
    private async void Delete(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepository = scope.ServiceProvider.GetRequiredService<MenuService>();
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepository.RemoveAsync(menu.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not remove record from DB {ex.Message}");
                }
            }
            else if (message is ProductMessage)
            {
                try
                {
                    var productRepository = scope.ServiceProvider.GetRequiredService<ProductService>();
                    var product = _mapper.Map<Product>(message);
                    await productRepository.RemoveAsync(product.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not remove record from DB {ex.Message}");
                }
            }
            else
            {
                try
                {
                    var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepository.RemoveAsync(reservation.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not remove record from DB {ex.Message}");
                }
            }
        }
    }
}


enum EventType
{
    Publish,
    Add,
    Update,
    Delete,
    Undetermined
}