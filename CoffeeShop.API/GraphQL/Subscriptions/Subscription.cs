using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class Subscription : ObjectGraphType
{
    public Subscription(
        MenuSubscriptionService menuService,
        ProductSubscriptionService productService,
        ReservationSubscriptionService reservationService)
    {
         AddField(new EventStreamFieldType
        {
            Name = "menuAdded",
            Type = typeof(MenuType),
            Resolver = new FuncFieldResolver<Menu>(c=> c.Source as Menu),
            Subscriber = new EventStreamResolver<Menu>(c=> menuService.GetAddedMenu())   
        });

        AddField(new EventStreamFieldType
        {
            Name = "menuUpdated",
            Type = typeof(MenuType),
            Resolver = new FuncFieldResolver<Menu>(c=> c.Source as Menu),
            Subscriber = new EventStreamResolver<Menu>(c=> menuService.GetUpdatedMenu())   
        });

        AddField(new EventStreamFieldType
        {
            Name = "menuRemoved",
            Type = typeof(MenuType),
            Resolver = new FuncFieldResolver<Menu>( c => c.Source as Menu),
            Subscriber = new EventStreamResolver<Menu>(c => menuService.GetRemovedMenu())
        });

        AddField(new EventStreamFieldType
        {
            Name = "productAdded",
            Type = typeof(ProductType),
            Resolver = new FuncFieldResolver<Product>( c => c.Source as Product),
            Subscriber = new EventStreamResolver<Product>(c => productService.GetAddedProduct())
        });

        AddField(new EventStreamFieldType
        {
            Name = "productUpdated",
            Type = typeof(ProductType),
            Resolver = new FuncFieldResolver<Product>(c => c.Source as Product),
            Subscriber = new EventStreamResolver<Product>(c => productService.GetUpdatedProduct())
        });

        AddField( new EventStreamFieldType
        {
            Name = "productRemoved",
            Type = typeof(ProductType),
            Resolver = new FuncFieldResolver<Product>(c => c.Source as Product),
            Subscriber = new EventStreamResolver<Product>( c => productService.GetRemovedProduct())
        });

        AddField(new EventStreamFieldType
        {
            Name = "reservationAdded",
            Type = typeof(ReservationType),
            Resolver = new FuncFieldResolver<Reservation>( c => c.Source as Reservation),
            Subscriber = new EventStreamResolver<Reservation>(c => reservationService.GetAddedReservation())
        });

        AddField(new EventStreamFieldType
        {
            Name = "reservationUpdated",
            Type = typeof(ReservationType),
            Resolver = new FuncFieldResolver<Reservation>(c => c.Source as Reservation),
            Subscriber = new EventStreamResolver<Reservation>(c => reservationService.GetUpdatedReservation())
        });

        AddField( new EventStreamFieldType
        {
            Name = "reservationRemoved",
            Type = typeof(ReservationType),
            Resolver = new FuncFieldResolver<Reservation>(c => c.Source as Reservation),
            Subscriber = new EventStreamResolver<Reservation>( c => reservationService.GetRemovedReservation())
        });
    }
}