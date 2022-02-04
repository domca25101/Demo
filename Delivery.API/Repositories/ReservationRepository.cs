using Delivery.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Delivery.API.Repositories;

public class ReservationRepository
{
    private readonly IMongoCollection<Reservation> _reservationCollection;
    public ReservationRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);
        _reservationCollection = mongoDatabase.GetCollection<Reservation>(
            dbSettings.Value.ReservationCollectionName);
    }

    public async Task<List<Reservation>> GetAsync() =>
       await _reservationCollection.Find(_ => true).ToListAsync();

    public async Task<Reservation> GetAsync(int id) =>
        await _reservationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Reservation reservation) =>
        await _reservationCollection.InsertOneAsync(reservation);

    public async Task UpdateAsync(int id, Reservation reservation) =>
        await _reservationCollection.ReplaceOneAsync(x => x.Id == id, reservation);

    public async Task RemoveAsync(int id) =>
        await _reservationCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> ReservationExist(int id) =>
        await _reservationCollection.Find(x => x.Id == id).AnyAsync();
}