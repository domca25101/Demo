using Delivery.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Delivery.API.Services;

public class MenuService
{
    private readonly IMongoCollection<Menu> _menuCollection;
    public MenuService(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);
        _menuCollection = mongoDatabase.GetCollection<Menu>(
            dbSettings.Value.MenuCollectionName);
    }

    public async Task<List<Menu>> GetAsync() =>
        await _menuCollection.Find(_ => true).ToListAsync();

    public async Task<Menu> GetAsync(int id) =>
        await _menuCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Menu menu) =>
        await _menuCollection.InsertOneAsync(menu);

    public async Task UpdateAsync(int id, Menu menu) =>
        await _menuCollection.ReplaceOneAsync(x => x.Id == id, menu);

    public async Task RemoveAsync(int id) =>
        await _menuCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> MenuExist(int id) =>
        await _menuCollection.Find(x => x.Id == id).AnyAsync();
}