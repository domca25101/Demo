namespace CoffeeShop.Client.Models;

public class Menu
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public List<Product> Products { get; set; }
}