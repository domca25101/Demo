using CoffeeShop.API.GraphQL.Subscribing;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using CoffeeShop.API.Repositories;
using GraphQL;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL;

public class Mutation : ObjectGraphType
{
    public Mutation(IMenuRepository menuRepository, IProductRepository productRepository,
     IReservationRepository reservationRepository, SubscriptionService subscriptionService)
    {   
        #region MenuMutation
        FieldAsync<MenuType>(
            "addMenu",
            arguments: new QueryArguments(new QueryArgument<MenuInputType> { Name = "menu" }),
            resolve: async context =>
            {
                
                var menu = context.GetArgument<Menu>("menu");
                subscriptionService.MenuAddedMessage(menu);
                return await menuRepository.AddMenu(menu);
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
                return await menuRepository.UpdateMenu(id, menu);
            }
        );
        FieldAsync<StringGraphType>(
            "removeMenu",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return await menuRepository.RemoveMenu(id);
            }
        );
        #endregion

        #region ProductMutation
        FieldAsync<ProductType>(
            "addProduct",
            arguments: new QueryArguments(new QueryArgument<ProductInputType> { Name = "product" }),
            resolve: async context =>
            {
                var product = context.GetArgument<Product>("product");
                return await productRepository.AddProduct(product);
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
                return await productRepository.UpdateProduct(id, product);
            }
        );
        FieldAsync<StringGraphType>(
            "removeProduct",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return await productRepository.RemoveProduct(id);
            }
        );
        #endregion

        #region ReservationMutation
        FieldAsync<ReservationType>(
            "addReservation",
            arguments: new QueryArguments(new QueryArgument<ReservationInputType> { Name = "reservation" }),
            resolve: async context =>
            {
                var reservation = context.GetArgument<Reservation>("reservation");
                return await reservationRepository.AddReservation(reservation);
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
                return await reservationRepository.UpdateReservation(id, reservation);
            }
        );
        FieldAsync<StringGraphType>(
            "removeReservation",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: async context =>
            {
                var id = context.GetArgument<int>("id");
                return await reservationRepository.RemoveReservation(id);
            }
        );
        #endregion
    }
}