using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL;

public class Query : ObjectGraphType
{
    public Query(IMenuRepository menuRepository, IProductRepository productRepository, IReservationRepository reservationRepository)
    {
        #region MenuQueries
        Field<ListGraphType<MenuType>>(
            "menus",
            resolve: context => { return menuRepository.GetMenus(); }
        );
        Field<MenuType>(
            "menu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => { return menuRepository.GetMenuById(context.GetArgument<int>("id")); }
        );
        #endregion

        #region ProductQueries
        Field<ListGraphType<ProductType>>(
           "products",
           resolve: context => { return productRepository.GetProducts(); }
       );
        Field<ListGraphType<ProductType>>(
            "productsForMenu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "menuId" }),
            resolve: context => { return productRepository.GetProductsForMenu(context.GetArgument<int>("menuId")); }
        );
        #endregion

        #region ReservationQueries
        Field<ListGraphType<ReservationType>>(
            "reservations",
            resolve: context => { return reservationRepository.GetReservations(); }
        );
        Field<ReservationType>(
            "reservation",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => { return reservationRepository.GetReservationById(context.GetArgument<int>("id")); }
        );
        #endregion
    }
}