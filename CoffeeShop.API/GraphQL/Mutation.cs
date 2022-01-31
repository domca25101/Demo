using CoffeeShop.API.GraphQL.Subscriptions;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL;

public class Mutation : ObjectGraphType
{
    public Mutation(
        IMenuRepository menuRepository, MenuSubscriptionService menuService,
        IProductRepository productRepository, ProductSubscriptionService productService,
        IReservationRepository reservationRepository, ReservationSubscriptionService reservationService)
    {
        #region MenuMutations
        FieldAsync<MenuType>(
            "addMenu",
            arguments: new QueryArguments(new QueryArgument<MenuInputType> { Name = "menu" }),
            resolve: async context =>
            {

                var menu = context.GetArgument<Menu>("menu");
                return menuService.MenuAdded(await menuRepository.AddMenu(menu)); ;
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
                return menuService.MenuUpdated(await menuRepository.UpdateMenu(id, menu));
            }
        );
        FieldAsync<MenuType>(
            "removeMenu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return menuService.MenuDeleted(await menuRepository.RemoveMenu(id));
            }
        );
        #endregion

        #region ProductMutations
        FieldAsync<ProductType>(
            "addProduct",
            arguments: new QueryArguments(new QueryArgument<ProductInputType> { Name = "product" }),
            resolve: async context =>
            {
                var product = context.GetArgument<Product>("product");
                return productService.ProductAdded(await productRepository.AddProduct(product));
            }
        );
        FieldAsync<ProductType>(
            "updateProduct",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<ProductInputType> { Name = "product" }
            ),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                var product = context.GetArgument<Product>("product");
                return productService.ProductUpdated(await productRepository.UpdateProduct(id, product));
            }
        );
        FieldAsync<ProductType>(
            "removeProduct",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return productService.ProductDeleted(await productRepository.RemoveProduct(id));
            }
        );
        #endregion

        #region ReservationMutations
        FieldAsync<ReservationType>(
           "addReservation",
           arguments: new QueryArguments(new QueryArgument<ReservationInputType> { Name = "reservation" }),
           resolve: async context =>
           {
               var reservation = context.GetArgument<Reservation>("reservation");
               return reservationService.ReservationAdded(await reservationRepository.AddReservation(reservation));
           }
       );
        FieldAsync<ReservationType>(
            "updateReservation",
            arguments: new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "id" },
                new QueryArgument<ReservationInputType> { Name = "reservation" }
            ),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                var reservation = context.GetArgument<Reservation>("reservation");
                return reservationService.ReservationUpdated(await reservationRepository.UpdateReservation(id, reservation));
            }
        );
        FieldAsync<ReservationType>(
            "removeReservation",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return reservationService.ReservationDeleted(await reservationRepository.RemoveReservation(id));
            }
        );
        #endregion
    }
}