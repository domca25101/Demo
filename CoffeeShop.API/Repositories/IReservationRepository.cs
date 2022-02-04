using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.Repositories;

public interface IReservationRepository
{
     Task<IEnumerable<Reservation>> GetReservations();
    Task<Reservation> GetReservationById(int id);
    Task<Reservation> AddReservation(Reservation reservation);
    Task<Reservation> UpdateReservation(int id, Reservation reservation);
    Task<IdModel> RemoveReservation(int id);
}