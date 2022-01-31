using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Mutations;

public class Mutation : ObjectGraphType
{
    public Mutation()
    {
        Field<MenuMutation>("menuMutation", resolve: context => new { });
        Field<ProductMutation>("productMutation", resolve: context => new { });
        Field<ReservationMutation>("reservationMutation", resolve: context => new { });
    }
}