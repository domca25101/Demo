using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Delivery.API.Models;

public class Product
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public int MenuId { get; set; }
}