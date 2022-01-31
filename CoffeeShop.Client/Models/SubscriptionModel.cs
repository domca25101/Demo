namespace CoffeeShop.Client.Models;

public class SubscriptionModel
{
    public Menu menuAdded { get; set; }
    public Menu menuUpdated { get; set; }
    public Menu menuRemoved { get; set; }

    public Product productAdded { get; set; }
    public Product productUpdated { get; set; }
    public Product productRemoved { get; set; }

    public Reservation reservationAdded { get; set; }
    public Reservation reservationUpdated { get; set; }
    public Reservation reservationRemoved { get; set; }
}