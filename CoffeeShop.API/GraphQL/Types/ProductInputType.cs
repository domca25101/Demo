using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class ProductInputType : InputObjectGraphType
{
    public ProductInputType()
    {
        Field<StringGraphType>("name");
        Field<StringGraphType>("description");
        Field<StringGraphType>("imageUrl");
        Field<FloatGraphType>("price");
        Field<IntGraphType>("menuId");
    }
}