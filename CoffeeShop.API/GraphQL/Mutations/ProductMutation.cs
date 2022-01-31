using CoffeeShop.API.GraphQL.Subscriptions;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Mutations;

public class ProductMutation : ObjectGraphType
{
    public ProductMutation(IProductRepository repository, ProductSubscriptionService service)
    {
        FieldAsync<ProductType>(
            "addProduct",
            arguments: new QueryArguments(new QueryArgument<ProductInputType> { Name = "product" }),
            resolve: async context =>
            {
                var product = context.GetArgument<Product>("product");
                return service.ProductAdded(await repository.AddProduct(product));
            }
        );
        FieldAsync<ProductType>(
            "updateProduct",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<ProductInputType> { Name = "product" }
            ),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                var product = context.GetArgument<Product>("product");
                return service.ProductUpdated(await repository.UpdateProduct(id, product));
            }
        );
        FieldAsync<ProductType>(
            "removeProduct",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return service.ProductDeleted(await repository.RemoveProduct(id));
            }
        );
    }
}