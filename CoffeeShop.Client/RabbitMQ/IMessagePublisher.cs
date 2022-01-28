using CoffeeShop.Client.Models;

namespace CoffeeShop.Client.RabbitMQ;

public interface IMessagePublisher
{
    Task PublishMenu(Menu menu, string eventType);
    Task PublishProduct(Product product, string eventType);
    Task PublishReservation(Reservation reservation, string eventType);
    Task Delete(int id, string entityType);
}