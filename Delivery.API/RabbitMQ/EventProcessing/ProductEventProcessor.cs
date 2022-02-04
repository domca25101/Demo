using AutoMapper;
using Delivery.API.Models;
using Delivery.API.Repositories;
using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public class ProductEventProcessor : EventProcessor
{
    private readonly IMapper _mapper;
    private readonly ProductRepository _repository;
    private readonly ILogger<ProductEventProcessor> _logger;

    public ProductEventProcessor(IMapper mapper, ProductRepository repository, ILogger<ProductEventProcessor> logger) : base(mapper, logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async void Add(Message message)
    {
        try
        {
            var product = _mapper.Map<Product>(message);
            await _repository.CreateAsync(product);
            _logger.LogInformation("Product: Event 'ADD' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Product: Could not process event 'ADD' successfully: {ex.Message}");
        }
    }

    public override async void Delete(Message message)
    {
        try
        {
            var product = _mapper.Map<Product>(message);
            await _repository.RemoveAsync(product.Id);
            _logger.LogInformation("Product: Event 'DELETE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Product: Could not process event 'DELETE' successfully: {ex.Message}");
        }
    }

    public override async void Update(Message message)
    {
        try
        {
            var product = _mapper.Map<Product>(message);
            await _repository.UpdateAsync(product.Id, product);
            _logger.LogInformation("Product: Event 'UPDATE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Product: Could not process event 'UPDATE' successfully: {ex.Message}");
        }
    }
}