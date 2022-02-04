using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class IdModel
{
    public Nullable<int> Id { get; set; }
}

public class IdType : ObjectGraphType<IdModel>
{
    public IdType()
    {
        Field(i => i.Id, nullable: true, type: typeof(IntGraphType));
    }
}