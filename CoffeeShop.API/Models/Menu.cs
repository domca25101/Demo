using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.API.Models;

public class Menu
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}