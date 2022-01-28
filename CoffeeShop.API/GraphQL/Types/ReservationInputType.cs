using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class ReservationInputType : InputObjectGraphType
{
    public ReservationInputType()
    {
        Field<StringGraphType>("name");
        Field<StringGraphType>("phone");
        Field<StringGraphType>("email");
        Field<StringGraphType>("date");
        Field<StringGraphType>("time");
        Field<IntGraphType>("totalPeople");
    }
}