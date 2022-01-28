using CoffeeShop.API.GraphQL.Subscribing;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL;

public class Subscription : ObjectGraphType
{
    public Subscription(SubscriptionService subscriptionService)
    {
        Name = "Subscription";
        AddField(new EventStreamFieldType
        {
            Name = "menuAdded",
            Type = typeof(MenuType),
            Resolver = new FuncFieldResolver<Menu>(c=> c.Source as Menu),
            Subscriber = new EventStreamResolver<Menu>(c=> subscriptionService.GetMessages())   
        });
    }
}