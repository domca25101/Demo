using AutoMapper;
using CoffeeShop.Client.Models;
using MessageModels;

namespace CoffeeShop.Client.RabbitMQ.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Menu, MenuMessage>();
        CreateMap<Product, ProductMessage>();
        CreateMap<Reservation, ReservationMessage>();
    }
}