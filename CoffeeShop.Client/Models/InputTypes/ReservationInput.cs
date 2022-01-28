namespace CoffeeShop.Client.Models.InputTypes;

public class ReservationInput
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int TotalPeople { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
}