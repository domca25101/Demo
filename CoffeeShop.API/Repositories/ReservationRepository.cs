using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _dbContext;

    public ReservationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Reservation> AddReservation(Reservation reservation)
    {
        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync();
        return reservation;

    }

    public async Task<Reservation> GetReservationById(int id)
    {
        return await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        return await _dbContext.Reservations.ToListAsync();
    }

    public async Task<string> RemoveReservation(int id)
    {
        var reservationObj = await GetReservationById(id);
        _dbContext.Reservations.Remove(reservationObj);
        await _dbContext.SaveChangesAsync();
        return $"Reservation with ID {id} was deleted!";
    }

    public async Task<Reservation> UpdateReservation(int id, Reservation reservation)
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
        return reservationObj;

    }
}