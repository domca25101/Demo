using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.API.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public int MenuId { get; set; }
    public Menu Menu { get; set; }
}