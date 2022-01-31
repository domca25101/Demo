using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public class SubscriptionHandler : IDisposable
{
    private readonly IGraphQLClient _client;
    private readonly IMessagePublisher _publisher;

    private readonly List<IDisposable> subscriptions = new List<IDisposable>();

    public SubscriptionHandler(IGraphQLClient client, IMessagePublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public SubscriptionHandler SubscribeAll()
    {
        subscriptions.Add(GQLSubscription.AddMenuSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.UpdateMenuSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.RemoveMenuSubscription(_client, _publisher));

        subscriptions.Add(GQLSubscription.AddProductSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.UpdateProductSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.RemoveProductSubscription(_client, _publisher));

        subscriptions.Add(GQLSubscription.AddReservationSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.UpdateReservationSubscription(_client, _publisher));
        subscriptions.Add(GQLSubscription.RemoveReservationSubscription(_client, _publisher));

        return this;
    }

    public void Dispose()
    {
        foreach (var subscription in subscriptions)
            subscription.Dispose();
    }
}