using AutoMapper;
using Delivery.API.Models;
using Delivery.API.Repositories;
using MessageModels;

namespace Delivery.API.RabbitMQ.EventProcessing;

public class ReservationEventProcessor : EventProcessor
{
    private readonly IMapper _mapper;
    private readonly ReservationRepository _repository;
    private readonly ILogger<ReservationEventProcessor> _logger;

    public ReservationEventProcessor(IMapper mapper, ReservationRepository repository, ILogger<ReservationEventProcessor> logger) : base(mapper, logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger =logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async void Add(Message message)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(message);
            await _repository.CreateAsync(reservation);
            _logger.LogInformation("Reservation: Event 'ADD' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Reservation: Could not process event 'ADD' successfully: {ex.Message}");
        }
    }

    public override async void Delete(Message message)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(message);
            await _repository.RemoveAsync(reservation.Id);
            _logger.LogInformation("Reservation: Event 'DELETE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Reservation: Could not process event 'DELETE' successfully: {ex.Message}");
        }
    }

    public override async void Update(Message message)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(message);
            await _repository.UpdateAsync(reservation.Id, reservation);
            _logger.LogInformation("Reservation: Event 'UPDATE' processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Reservation: Could not process event 'UPDATE' successfully: {ex.Message}");
        }
    }
}