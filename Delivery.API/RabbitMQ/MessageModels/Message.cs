namespace MessageModels;

public abstract class Message
{
     public int Id { get; set; }
    public string Event { get; set; }
}