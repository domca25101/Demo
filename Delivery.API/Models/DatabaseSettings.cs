namespace Delivery.API.Models;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string MenuCollectionName { get; set; } = null!;
    public string ProductCollectionName { get; set; } = null!;
    public string ReservationCollectionName { get; set; } = null!;
}