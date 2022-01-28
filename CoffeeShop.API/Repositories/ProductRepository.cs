using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsForMenu(int menuId)
    {
        return await _dbContext.Products.Where(p => p.MenuId == menuId).ToListAsync();
    }

    public async Task<string> RemoveProduct(int id)
    {
        var productObj = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        _dbContext.Products.Remove(productObj);
        await _dbContext.SaveChangesAsync();
        return $"Product with ID {id} was deleted!";
    }

    public async Task<Product> UpdateProduct(int id, Product product)
    {
        var productObj = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        productObj.Name = product.Name;
        productObj.Description = product.Description;
        productObj.Price = product.Price;
        productObj.ImageUrl = product.ImageUrl;
        productObj.MenuId = product.MenuId;

        _dbContext.Products.Update(productObj);
        await _dbContext.SaveChangesAsync();
        return productObj;
    }
}