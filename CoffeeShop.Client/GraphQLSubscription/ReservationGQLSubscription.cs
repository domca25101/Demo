using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public static class ReservationGQLSubscription
{
    public static IDisposable AddReservationSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishReservation(response.Data.reservationAdded, "Add"));
            async response =>
            {
                if (response.Data.reservationAdded != null)
                {
                    try
                    {
                        await publisher.SendReservation(response.Data.reservationAdded, "Add");
                        logger.LogInformation("Reservation Message (ADD) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Reservation Message (ADD) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no reservation to add.");
                }
            });

        return subscription;
    }

    public static IDisposable UpdateReservationSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishReservation(response.Data.reservationUpdated, "Update"));
            async response =>
            {
                if (response.Data.reservationUpdated != null)
                {
                    try
                    {
                        await publisher.SendReservation(response.Data.reservationUpdated, "Update");
                        logger.LogInformation("Reservation Message (UPDATE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Reservation Message (UPDATE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no reservation to update.");
                }
            });
        return subscription;
    }

    public static IDisposable RemoveReservationSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishReservation(response.Data.reservationRemoved, "Delete"));
            async response =>
            {
                if (response.Data.reservationRemoved != null)
                {
                    try
                    {
                        var data = new Reservation
                        {
                            Id = (int)response.Data.reservationRemoved.Id
                        };
                        await publisher.SendReservation(data, "Delete");
                        logger.LogInformation("Reservation Message (DELETE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Reservation Message (DELETE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no reservation to remove.");
                }
            });
        return subscription;
    }
}