using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class ProductType : ObjectGraphType<Product>
{
    public ProductType(AppDbContext dbContext)
    {
        Field(p => p.Id);
        Field(p => p.Name);
        Field(p => p.Description);
        Field(p => p.Price);
        Field(p => p.ImageUrl);
        Field(p => p.MenuId);
        Field<MenuType>("menu", resolve: context => { return dbContext.Menus.FirstOrDefault(m => m.Id == context.Source.MenuId); });
    }
}