using CoffeeShop.API.GraphQL.Subscriptions;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Mutations;

public class ReservationMutation : ObjectGraphType
{
    public ReservationMutation(IReservationRepository repository, ReservationSubscriptionService service)
    {
        FieldAsync<ReservationType>(
            "addReservation",
            arguments: new QueryArguments(new QueryArgument<ReservationInputType> { Name = "reservation" }),
            resolve: async context =>
            {
                var reservation = context.GetArgument<Reservation>("reservation");
                return service.ReservationAdded(await repository.AddReservation(reservation));
            }
        );
        FieldAsync<ReservationType>(
            "updateReservation",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<ReservationInputType> { Name = "reservation" }
            ),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                var reservation = context.GetArgument<Reservation>("reservation");
                return service.ReservationUpdated(await repository.UpdateReservation(id, reservation));
            }
        );
        FieldAsync<ReservationType>(
            "removeReservation",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return service.ReservationDeleted(await repository.RemoveReservation(id));
            }
        );
    }
}