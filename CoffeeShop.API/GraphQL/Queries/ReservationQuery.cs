using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Queries;

public class ReservationQuery : ObjectGraphType
{
    public ReservationQuery(IReservationRepository repository)
    {
        Field<ListGraphType<ReservationType>>(
            "reservations",
            resolve: context => { return repository.GetReservations(); }
        );
        Field<ReservationType>(
            "reservation",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => { return repository.GetReservationById(context.GetArgument<int>("id")); }
        );
    }
}