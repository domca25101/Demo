using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using EasyNetQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public static class GQLSubscription
{
    //Subscription requests for menus
    #region MenuSubscribtions
    public static IDisposable AddMenuSubscription(IGraphQLClient client, IMessagePublisher publisher)
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

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishMenu(response.Data.menuAdded, "Add"));

        return subscription;
    }

    public static IDisposable UpdateMenuSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                menuUpdated{
                    id
                    name
                    imageUrl
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishMenu(response.Data.menuUpdated, "Update"));

        return subscription;
    }

    public static IDisposable RemoveMenuSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                menuRemoved{
                    id
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishMenu(response.Data.menuRemoved, "Delete"));

        return subscription;
    }
    #endregion

    //Subscription requests for products
    #region ProductSubscriptions
    public static IDisposable AddProductSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                productAdded{
                    id
                    name
                    description
                    price
                    imageUrl
                    menuId
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishProduct(response.Data.productAdded, "Add"));

        return subscription;
    }

    public static IDisposable UpdateProductSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                productUpdated{
                    id
                    name
                    description
                    price
                    imageUrl
                    menuId
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishProduct(response.Data.productUpdated, "Update"));

        return subscription;
    }

    public static IDisposable RemoveProductSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                productRemoved{
                    id
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishProduct(response.Data.productRemoved, "Delete"));

        return subscription;
    }
    #endregion

    //Subscription requests for reservations
    #region ReservationSubscriptions
    public static IDisposable AddReservationSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                reservationAdded{
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishReservation(response.Data.reservationAdded, "Add"));

        return subscription;
    }

    public static IDisposable UpdateReservationSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                reservationUpdated{
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishReservation(response.Data.reservationUpdated, "Update"));

        return subscription;
    }

    public static IDisposable RemoveReservationSubscription(IGraphQLClient client, IMessagePublisher publisher)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            subscription{
                reservationRemoved{
                    id
                }
            }"
        };

        var subscriptionStream = client.CreateSubscriptionStream<SubscriptionModel>(request);
        var subscription = subscriptionStream.Subscribe(
            async response => await publisher.PublishReservation(response.Data.reservationRemoved, "Delete"));

        return subscription;
    }
    #endregion
}