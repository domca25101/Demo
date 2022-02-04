using CoffeeShop.API.Data;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ReservationRepository> _logger;

    public ReservationRepository(AppDbContext dbContext, ILogger<ReservationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Reservation> AddReservation(Reservation reservation)
    {
        try
        {
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Reservation added to DB.");
            return reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not add Reservation to DB: {ex.Message}");
            return null;
        }
    }

    public async Task<Reservation> GetReservationById(int id)
    {
        try
        {
            var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            _logger.LogInformation("Query 'reservation' was successful.");
            return reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch reservation with ID {id} from DB: {ex.Message}");
            return new Reservation
            {
                Id = 0,
                Name = "",
                Phone = "",
                Email = "",
                TotalPeople = 0,
                Date = "",
                Time = null
            };
        }
    }

    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        try
        {
            var reservations = await _dbContext.Reservations.ToListAsync();
            _logger.LogInformation("Query 'reservations' was successful.");
            return reservations;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch reservations from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<IdModel> RemoveReservation(int id)
    {
        try
        {
            var reservationObj = await GetReservationById(id);
            _dbContext.Reservations.Remove(reservationObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation with ID {id} removed from DB");
            var idObj = new IdModel { Id = reservationObj.Id };
            return idObj;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not remove Reservation with ID {id} from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<Reservation> UpdateReservation(int id, Reservation reservation)
    {
        try
        {
            var reservationObj = await GetReservationById(id);
            reservationObj.Name = reservation.Name;
            reservationObj.Phone = reservation.Phone;
            reservationObj.Email = reservation.Email;
            reservationObj.TotalPeople = reservation.TotalPeople;
            reservationObj.Date = reservation.Date;
            reservationObj.Time = reservation.Time;
            _dbContext.Reservations.Update(reservationObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation with ID {id} updated in DB.");
            return reservationObj;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Could not update Reservation with ID {id} in DB: {ex.Message}");
            return null;
        }
    }
}