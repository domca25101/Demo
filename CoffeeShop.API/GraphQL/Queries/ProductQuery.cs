using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Queries;

public class ProductQuery : ObjectGraphType
{
    public ProductQuery(IProductRepository repository)
    {
          Field<ListGraphType<ProductType>>(
            "products",
            resolve: context => { return repository.GetProducts(); }
        );
        Field<ListGraphType<ProductType>>(
            "productsForMenu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "menuId" }),
            resolve: context => { return repository.GetProductsForMenu(context.GetArgument<int>("menuId")); }
        );
    }
}