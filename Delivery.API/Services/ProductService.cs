using Delivery.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Delivery.API.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductService(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);
        _productCollection = mongoDatabase.GetCollection<Product>(
            dbSettings.Value.ProductCollectionName);
    }

    public async Task<List<Product>> GetAsync() =>
        await _productCollection.Find(_ => true).ToListAsync();

    public async Task<List<Product>> GetByMenuIdAsync(int menuId) =>
        await _productCollection.Find(x => x.MenuId == menuId).ToListAsync();

    public async Task CreateAsync(Product product) =>
        await _productCollection.InsertOneAsync(product);

    public async Task UpdateAsync(int id, Product product) =>
        await _productCollection.ReplaceOneAsync(x => x.Id == id, product);

    public async Task RemoveAsync(int id) =>
        await _productCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<bool> ProductExist(int id) =>
        await _productCollection.Find(x => x.Id == id).AnyAsync();
}