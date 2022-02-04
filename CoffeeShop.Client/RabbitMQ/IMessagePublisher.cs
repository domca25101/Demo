using CoffeeShop.Client.Models;

namespace CoffeeShop.Client.RabbitMQ;

public interface IMessagePublisher
{
    Task SendMenu(Menu menu, string eventType);
    Task SendProduct(Product product, string eventType);
    Task SendReservation(Reservation reservation, string eventType);
}