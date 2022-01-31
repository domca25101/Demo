using CoffeeShop.API.GraphQL.Queries;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Queries;

public class Query : ObjectGraphType
{
    public Query()
    {
        Field<MenuQuery>("menuQuery", resolve: context => new { });
        Field<ProductQuery>("productQuery", resolve: context => new { });
        Field<ReservationQuery>("reservationQuery", resolve: context => new { });
    }
}