using CoffeeShop.Client.Models;
using CoffeeShop.Client.RabbitMQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.GraphQLSubscription;

public static class ProductGQLSubscription
{
    public static IDisposable AddProductSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishProduct(response.Data.productAdded, "Add"));
            async response =>
            {
                if (response.Data.productAdded != null)
                {
                    try
                    {
                        await publisher.SendProduct(response.Data.productAdded, "Add");
                        logger.LogInformation("Product Message (ADD) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Product Message (ADD) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no product to add.");
                }
            });
        return subscription;
    }

    public static IDisposable UpdateProductSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishProduct(response.Data.productUpdated, "Update"));
            async response =>
            {
                if (response.Data.productUpdated != null)
                {
                    try
                    {
                        await publisher.SendProduct(response.Data.productUpdated, "Update");
                        logger.LogInformation("Product Message (UPDATE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Product Message (UPDATE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no product to update.");
                }
            });
        return subscription;
    }

    public static IDisposable RemoveProductSubscription(IGraphQLClient client, IMessagePublisher publisher, ILogger logger)
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
            // async response => await publisher.PublishProduct(response.Data.productRemoved, "Delete"));
            async response =>
            {
                if (response.Data.productRemoved != null)
                {
                    try
                    {
                        var data = new Product
                        {
                            Id = (int)response.Data.productRemoved.Id
                        };
                        await publisher.SendProduct(data, "Delete");
                        logger.LogInformation("Product Message (DELETE) was sent successfully to queue.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Could not send Product Message (DELETE) to queue: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("There was no Product to remove.");
                }
            });
        return subscription;
    }
}