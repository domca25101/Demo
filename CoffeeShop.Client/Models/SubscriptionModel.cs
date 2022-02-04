namespace CoffeeShop.Client.Models;

public class SubscriptionModel
{
    public Menu menuAdded { get; set; }
    public Menu menuUpdated { get; set; }
    public IdModel menuRemoved { get; set; }

    public Product productAdded { get; set; }
    public Product productUpdated { get; set; }
    public IdModel productRemoved { get; set; }

    public Reservation reservationAdded { get; set; }
    public Reservation reservationUpdated { get; set; }
    public IdModel reservationRemoved { get; set; }
}