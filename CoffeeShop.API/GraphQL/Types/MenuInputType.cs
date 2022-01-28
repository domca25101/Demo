using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class MenuInputType : InputObjectGraphType
{
    public MenuInputType()
    {
        Field<StringGraphType>("name");
        Field<StringGraphType>("imageUrl");
    }
}