using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Delivery.API.Models;

public class Menu
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}