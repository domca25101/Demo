using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<IEnumerable<Product>> GetProductsForMenu(int menuId);
    Task<Product> AddProduct(Product product);
    Task<Product> UpdateProduct(int id, Product product);
    Task<IdModel> RemoveProduct(int id);
}