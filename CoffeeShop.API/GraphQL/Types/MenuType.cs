using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class MenuType : ObjectGraphType<Menu>
{
    public MenuType(AppDbContext dbContext, ILogger<MenuType> logger)
    {
        Field(p => p.Id);
        Field(p => p.Name);
        Field(p => p.ImageUrl);
        Field<ListGraphType<ProductType>>("products", resolve: context =>
        {
            try
            {
                var products = dbContext.Products.Where(p => p.MenuId == context.Source.Id).ToList();
                logger.LogInformation("Query 'products' was successful");
                return products;
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not fetch products for menu with ID {context.Source.Id} from DB: {ex.Message}");
                return null;
            }
        });
    }
}