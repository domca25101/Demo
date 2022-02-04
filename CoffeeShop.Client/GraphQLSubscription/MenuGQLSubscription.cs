using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public static class MenuGQLSubscription
{
    public static IDisposable AddMenuSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
        // async response => await publisher.PublishMenu(response.Data.menuAdded, "Add"));
        async response =>
        {
            if (response.Data.menuAdded != null)
            {
                try
                {
                    await publisher.SendMenu(response.Data.menuAdded, "Add");
                    logger.LogInformation("Menu Message (ADD) was sent successfully to queue.");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Could not send Menu Message (ADD) to queue: {ex.Message}");
                }
            }
            else
            {
                logger.LogInformation("There was no menu to add.");
            }
        });
        return subscription;
    }

    public static IDisposable UpdateMenuSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishMenu(response.Data.menuUpdated, "Update"));
            async response =>
            {
                if (response.Data.menuUpdated != null)
                {
                    try
                    {
                        await publisher.SendMenu(response.Data.menuUpdated, "Update");
                        logger.LogInformation("Menu Message (UPDATE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Menu Message (UPDATE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no menu to update.");
                }
            });
        return subscription;
    }

    public static IDisposable RemoveMenuSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishMenu(response.Data.menuRemoved, "Delete"));
            async response =>
            {
                if (response.Data.menuRemoved != null)
                {
                    try
                    {
                        var data = new Menu
                        {
                            Id = (int)response.Data.menuRemoved.Id
                        };
                        await publisher.SendMenu(data, "Delete");
                        logger.LogInformation("Menu Message (DELETE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Menu Message (DELETE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no Menu to remove.");
                }
            });
        return subscription;
    }
}