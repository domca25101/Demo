namespace MessageModels;

public class DeleteMessage
{
    public int Id { get; set; }
    public string Event { get; set; }
    public string EntityType { get; set; }
}