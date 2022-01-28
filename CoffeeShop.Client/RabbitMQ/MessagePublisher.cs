using AutoMapper;
using CoffeeShop.Client.Models;
using EasyNetQ;
using MessageModels;

namespace CoffeeShop.Client.RabbitMQ;

public class MessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public MessagePublisher(IBus bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }

    public async Task Delete(int id, string entityType)
    {
        var message = new DeleteMessage();
        message.Id = id;
        message.Event = "Delete";
        message.EntityType = entityType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
        Console.WriteLine("--> Delete Message published!");
    }

    public async Task PublishMenu(Menu menu, string eventType)
    {
        var message = _mapper.Map<MenuMessage>(menu);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
        Console.WriteLine("--> Menu Message published!");
    }

    public async Task PublishProduct(Product product, string eventType)
    {
        var message = _mapper.Map<ProductMessage>(product);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
        Console.WriteLine("--> Product Message published!");
    }

    public async Task PublishReservation(Reservation reservation, string eventType)
    {
        var message = _mapper.Map<ReservationMessage>(reservation);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
        Console.WriteLine("--> Reservation Message published!");
    }
}