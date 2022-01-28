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

    public void ProcessEvent(Message message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.Publish:
                Publish(message);
                break;
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

    private EventType DetermineEvent(Message message)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = _mapper.Map<GenericEvent>(message);
        switch (eventType.Event)
        {
            case "Publish":
                Console.WriteLine("--> Publish Event Determined");
                return EventType.Publish;
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

    private async void Publish(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepo = scope.ServiceProvider.GetRequiredService<MenuService>();
                    if (menuRepo.MenuExist(message.Id))
                    {
                        Console.WriteLine("--> Menu already exists!");
                        return;
                    }
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepo.CreateAsync(menu);
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
                    var productRepo = scope.ServiceProvider.GetRequiredService<ProductService>();
                    if (productRepo.ProductExist(message.Id))
                    {
                        Console.WriteLine("--> Product already exists!");
                        return;
                    }
                    var product = _mapper.Map<Product>(message);
                    await productRepo.CreateAsync(product);
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
                    var reservationRepo = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    if (reservationRepo.ReservationExist(message.Id))
                    {
                        Console.WriteLine("--> Reservation already exists!");
                        return;
                    }
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepo.CreateAsync(reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add to DB {ex.Message}");
                }
            }
        }
    }

    private async void Add(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepo = scope.ServiceProvider.GetRequiredService<MenuService>();
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepo.CreateAsync(menu);
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
                    var productRepo = scope.ServiceProvider.GetRequiredService<ProductService>();
                    var product = _mapper.Map<Product>(message);
                    await productRepo.CreateAsync(product);
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
                    var reservationRepo = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepo.CreateAsync(reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add to DB {ex.Message}");
                }
            }
        }
    }

    private async void Update(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            if (message is MenuMessage)
            {
                try
                {
                    var menuRepo = scope.ServiceProvider.GetRequiredService<MenuService>();
                    var menu = _mapper.Map<Menu>(message);
                    await menuRepo.UpdateAsync(menu.Id, menu);
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
                    var productRepo = scope.ServiceProvider.GetRequiredService<ProductService>();
                    var product = _mapper.Map<Product>(message);
                    await productRepo.UpdateAsync(product.Id, product);
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
                    var reservationRepo = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    var reservation = _mapper.Map<Reservation>(message);
                    await reservationRepo.UpdateAsync(reservation.Id, reservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update DB {ex.Message}");
                }
            }
        }
    }

    private async void Delete(Message message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var msg = (DeleteMessage)message;
            if (msg.EntityType == "Menu")
            {
                try
                {
                    var menuRepo = scope.ServiceProvider.GetRequiredService<MenuService>();
                    await menuRepo.RemoveAsync(msg.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete record from DB {ex.Message}");
                }
            }
            else if (msg.EntityType == "Product")
            {
                try
                {
                    var productRepo = scope.ServiceProvider.GetRequiredService<ProductService>();
                    await productRepo.RemoveAsync(msg.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete record from DB {ex.Message}");
                }
            }
            else
            {
                try
                {
                    var reservationRepo = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    await reservationRepo.RemoveAsync(msg.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete record from DB {ex.Message}");
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