namespace MessageModels;

public class ReservationMessage : Message
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int TotalPeople { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
}