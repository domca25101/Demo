using CoffeeShop.API.GraphQL.Subscriptions;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Mutations;

public class MenuMutation : ObjectGraphType
{
    public MenuMutation(IMenuRepository repository, MenuSubscriptionService service)
    {
        FieldAsync<MenuType>(
            "addMenu",
            arguments: new QueryArguments(new QueryArgument<MenuInputType> { Name = "menu" }),
            resolve: async context =>
            {

                var menu = context.GetArgument<Menu>("menu");
                return service.MenuAdded(await repository.AddMenu(menu));;
            }
        );
        FieldAsync<MenuType>(
            "updateMenu",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<MenuInputType> { Name = "menu" }
            ),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                var menu = context.GetArgument<Menu>("menu");
                return service.MenuUpdated(await repository.UpdateMenu(id, menu));
            }
        );
        FieldAsync<MenuType>(
            "removeMenu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return service.MenuDeleted(await repository.RemoveMenu(id));
            }
        );
    }
}