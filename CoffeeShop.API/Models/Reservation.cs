using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.API.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public int TotalPeople { get; set; }
    [Required]
    public string Date { get; set; }
    [Required]
    public string Time { get; set; }
}