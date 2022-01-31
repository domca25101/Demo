using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Queries;

public class MenuQuery : ObjectGraphType{
    public MenuQuery(IMenuRepository repository)
    {
           Field<ListGraphType<MenuType>>(
            "menus",
            resolve: context => { return repository.GetMenus(); }
        );
        Field<MenuType>(
            "menu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => { return repository.GetMenuById(context.GetArgument<int>("id")); }
        );
    }
}