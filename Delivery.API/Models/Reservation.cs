using MongoDB.Bson.Serialization.Attributes;

namespace Delivery.API.Models;

public class Reservation
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int TotalPeople { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
}