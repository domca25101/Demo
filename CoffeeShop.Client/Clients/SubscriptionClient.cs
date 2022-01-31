using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.Clients;

public class SubscriptionClient
{
    private readonly IGraphQLClient _client;
    private readonly IMessagePublisher _publisher;

    public SubscriptionClient(IGraphQLClient client, IMessagePublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public void AddMenuSubscription()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
        subscription{
            menuAdded{
                id
                name
                imageUrl
            }
        }"
        };
        var subscriptionStream = _client.CreateSubscriptionStream<MenuSubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await _publisher.PublishMenu(response.Data.menuAdded, "Add"));
    }

}