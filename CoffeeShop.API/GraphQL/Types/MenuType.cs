using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class MenuType : ObjectGraphType<Menu>
{
    public MenuType( AppDbContext dbContext)
    {
        Field(p => p.Id);
        Field(p => p.Name);
        Field(p => p.ImageUrl);
        Field<ListGraphType<ProductType>>("products", resolve: context => { return dbContext.Products.Where(p => p.MenuId == context.Source.Id).ToList(); });
    }
}