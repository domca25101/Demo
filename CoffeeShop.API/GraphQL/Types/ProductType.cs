using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class ProductType : ObjectGraphType<Product>
{
    public ProductType(AppDbContext dbContext, ILogger<ProductType> logger)
    {
        Field(p => p.Id);
        Field(p => p.Name);
        Field(p => p.Description);
        Field(p => p.Price);
        Field(p => p.ImageUrl);
        Field(p => p.MenuId);
        Field<MenuType>("menu", resolve: context =>
        {
            try
            {
                var menu = dbContext.Menus.FirstOrDefault(m => m.Id == context.Source.MenuId);
                logger.LogInformation("Query 'Menu' was successful.");
                return menu;
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not fetch menu with ID {context.Source.MenuId} from DB: {ex.Message}");
                return new Menu
                {
                    Id = 0,
                    Name = "",
                    ImageUrl = ""
                };
            }
        });
    }
}