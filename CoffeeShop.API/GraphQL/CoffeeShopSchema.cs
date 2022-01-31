using CoffeeShop.API.GraphQL.Subscriptions;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL;

public class CoffeeShopSchema : Schema
{
    public CoffeeShopSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<Query>();
        Mutation = provider.GetRequiredService<Mutation>();
        Subscription = provider.GetRequiredService<Subscription>();
    }
}