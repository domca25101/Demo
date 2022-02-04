using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public class SubscriptionHandler : IDisposable
{
    private readonly IGraphQLClient _client;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<SubscriptionHandler> _logger;
    private readonly List<IDisposable> subscriptions = new List<IDisposable>();

    public SubscriptionHandler(IGraphQLClient client, IMessagePublisher publisher, ILogger<SubscriptionHandler> logger)
    {
        _client = client;
        _publisher = publisher;
        _logger = logger;
    }

    public SubscriptionHandler SubscribeAll()
    {
        subscriptions.Add(MenuGQLSubscription.AddMenuSubscription(_client, _publisher, _logger));
        subscriptions.Add(MenuGQLSubscription.UpdateMenuSubscription(_client, _publisher, _logger));
        subscriptions.Add(MenuGQLSubscription.RemoveMenuSubscription(_client, _publisher, _logger));

        subscriptions.Add(ProductGQLSubscription.AddProductSubscription(_client, _publisher, _logger));
        subscriptions.Add(ProductGQLSubscription.UpdateProductSubscription(_client, _publisher, _logger));
        subscriptions.Add(ProductGQLSubscription.RemoveProductSubscription(_client, _publisher, _logger));

        subscriptions.Add(ReservationGQLSubscription.AddReservationSubscription(_client, _publisher, _logger));
        subscriptions.Add(ReservationGQLSubscription.UpdateReservationSubscription(_client, _publisher, _logger));
        subscriptions.Add(ReservationGQLSubscription.RemoveReservationSubscription(_client, _publisher, _logger));

        return this;
    }

    public void Dispose()
    {
        foreach (var subscription in subscriptions)
            subscription.Dispose();
    }
}