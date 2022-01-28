using AutoMapper;
using Delivery.API.Models;
using MessageModels;

namespace Delivery.API.RabbitMQ.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, GenericEvent>();
        CreateMap<MenuMessage, Menu>();
        CreateMap<ProductMessage, Product>();
        CreateMap<ReservationMessage, Reservation>();
    }
}