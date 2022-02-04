using AutoMapper;
using Delivery.API.Models;
using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public abstract class EventProcessor : IEventProcessor
{
    private readonly IMapper _mapper;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IMapper mapper, ILogger<EventProcessor> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // public abstract void ProcessEvent(Message message);
    public abstract void Add(Message message);
    public abstract void Update(Message message);
    public abstract void Delete(Message message);

    /// <summary>
    /// Determines Type of the event
    /// </summary>
    /// <param name="platformPublishedDto"></param>
    /// <returns></returns>
    public EventType DetermineEvent(Message message)
    {
        var eventType = _mapper.Map<GenericEvent>(message);
        switch (eventType.Event)
        {
            case "Add":
                _logger.LogInformation("'ADD' event determined.");
                return EventType.Add;
            case "Update":
                _logger.LogInformation("'UPDATE' event determined.");
                return EventType.Update;
            case "Delete":
                _logger.LogInformation("'DELETE' event determined.");
                return EventType.Delete;
            default:
                _logger.LogInformation("Could not determine event.");
                return EventType.Undetermined;
        }
    }

    /// <summary>
    /// Processes message based on Event written inside message
    /// </summary>
    /// <param name="message"></param>
    public void ProcessEvent(Message message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.Add:
                Add(message);
                break;
            case EventType.Update:
                Update(message);
                break;
            case EventType.Delete:
                Delete(message);
                break;
            default:
                break;
        }
    }

}

public enum EventType
{
    Add,
    Update,
    Delete,
    Undetermined
}