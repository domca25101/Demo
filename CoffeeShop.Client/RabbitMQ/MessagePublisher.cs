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

    //Send Menu Message with specified event to defined Queue
    public async Task SendMenu(Menu menu, string eventType)
    {
        var message = _mapper.Map<MenuMessage>(menu);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
    }

    //Send Product Message with specified event to defined Queue
    public async Task SendProduct(Product product, string eventType)
    {
        var message = _mapper.Map<ProductMessage>(product);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
    }

    //Send Reservation Message with specified event to defined Queue
    public async Task SendReservation(Reservation reservation, string eventType)
    {
        var message = _mapper.Map<ReservationMessage>(reservation);
        message.Event = eventType;
        await _bus.SendReceive.SendAsync("CoffeeShop.Data", message);
    }
}