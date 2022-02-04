using AutoMapper;
using Delivery.API.Models;
using Delivery.API.Repositories;
using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public class MenuEventProcessor : EventProcessor
{
    private readonly IMapper _mapper;
    private readonly MenuRepository _repository;
    private readonly ILogger<MenuEventProcessor> _logger;

    public MenuEventProcessor(IMapper mapper, MenuRepository repository, ILogger<MenuEventProcessor> logger) : base(mapper, logger)
    {

        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async void Add(Message message)
    {
        try
        {
            var menu = _mapper.Map<Menu>(message);
            await _repository.CreateAsync(menu);
            _logger.LogInformation("Menu: Event 'ADD' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Menu: Could not process event 'ADD' successfully: {ex.Message}");
        }
    }

    public override async void Delete(Message message)
    {
        try
        {
            var menu = _mapper.Map<Menu>(message);
            await _repository.RemoveAsync(menu.Id);
            _logger.LogInformation("Menu: Event 'DELETE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Menu: Could not process event 'DELETE' successfully: {ex.Message}");
        }
    }


    public override async void Update(Message message)
    {
        try
        {
            var menu = _mapper.Map<Menu>(message);
            await _repository.UpdateAsync(menu.Id, menu);
            _logger.LogInformation("Menu: Event 'UPDATE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Menu: Could not process event 'UPDATE' successfully: {ex.Message}");
        }
    }
}