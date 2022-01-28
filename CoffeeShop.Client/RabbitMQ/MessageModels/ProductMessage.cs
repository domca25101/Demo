namespace MessageModels;

public class ProductMessage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public int MenuId { get; set; }
    public string Event { get; set; }
}