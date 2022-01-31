using CoffeeShop.Client.Models;
using CoffeeShop.Client.Models.InputTypes;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.Clients;

public class GraphQLClient
{
    private readonly IGraphQLClient _client;

    public GraphQLClient(IGraphQLClient client)
    {
        _client = client;
    }

    #region  Menu
    public async Task<IEnumerable<Menu>> GetMenus()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
                menuQuery{
                    menus{
                        id
                        name
                        imageUrl
                        products{
                            id
                            name
                            description
                            price
                            imageUrl
                            menuId
                        }    
                    }
                }
            }"
        };
        var response = await _client.SendQueryAsync(request, () => new { menus = new List<Menu>() });
        return response.Data.menus;
    }

    public async Task<Menu> GetMenuById(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($id: Int){
                menuQuery{
                    menu(id: $id){
                        id
                        name
                        imageUrl
                        products{
                            id
                            name
                            description
                            price
                            imageUrl
                            menuId
                        }
                    }
                }    
            }",
            Variables = new { id = id }
        };
        var response = await _client.SendQueryAsync(request, () => new { menu = new Menu() });
        return response.Data.menu;
    }

    public async Task<Menu> AddMenu(MenuInput menu)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($menu: MenuInputType)
            {
                menuMutation{
                    addMenu(menu: $menu)
                    {
                        id
                        name
                        imageUrl
                    }
                }
            }",
            Variables = new { menu = menu }
        };
        var response = await _client.SendMutationAsync(request, () => new { addMenu = new Menu() });
        return response.Data.addMenu;
    }

    public async Task<Menu> UpdateMenu(int id, MenuInput menu)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $menu: MenuInputType){
                menuMutation{
                    updateMenu(id: $id, menu :$menu){
                        id
                        name
                        imageUrl
                    }
                }
            }",
            Variables = new { id = id, menu = menu }
        };
        var response = await _client.SendMutationAsync(request, () => new { updateMenu = new Menu() });
        return response.Data.updateMenu;
    }

    public async Task<Menu> DeleteMenu(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                menuMutation{
                    removeMenu(id : $id){
                        id
                    }
                }
            }",
            Variables = new { id = id }
        };
        var response = await _client.SendMutationAsync(request, () => new { removeMenu = new Menu() });
        return response.Data.removeMenu;
    }
    #endregion

    #region Product
    public async Task<IEnumerable<Product>> GetProducts()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
                productQuery{
                    products{
                        id
                        name
                        imageUrl
                        description
                        price
                        menuId
                        menu
                    }
                }
            }"
        };
        var response = await _client.SendQueryAsync(request, () => new { products = new List<Product>() });
        return response.Data.products;
    }

    public async Task<IEnumerable<Product>> GetProductsForMenu(int menuId)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($menuId: Int){
                productQuery{
                    productsForMenu(menuId: $menuId){
                        id
                        name
                        imageUrl
                        description
                        price
                        menuId
                        menu{
                            name
                        }
                    }
                }    
            }",
            Variables = new { menuId = menuId }
        };
        var response = await _client.SendQueryAsync(request, () => new { productsForMenu = new List<Product>() });
        return response.Data.productsForMenu;
    }

    public async Task<Product> AddProduct(ProductInput product)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($product: ProductInputType){
                productMutation{
                    addProduct(product: $product){
                        id
                        name
                        description
                        price
                        imageUrl
                        menuId
                    }
                }
            }",
            Variables = new { product = product }
        };
        var response = await _client.SendMutationAsync(request, () => new { addProduct = new Product() });
        return response.Data.addProduct;
    }

    public async Task<Product> UpdateProduct(int id, ProductInput product)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $product: ProductInputType){
                productMutation{
                    updateProduct(id: $id, product :$product){
                        id
                        name
                        description
                        price
                        imageUrl
                        menuId
                    }
                }
            }",
            Variables = new { id = id, product = product }
        };
        var response = await _client.SendMutationAsync(request, () => new { updateProduct = new Product() });
        return response.Data.updateProduct;
    }

    public async Task<Product> DeleteProduct(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                productMutation{
                    removeProduct(id : $id){
                        id
                    }
                }   
            }",
            Variables = new { id = id }
        };
        var response = await _client.SendMutationAsync(request, () => new { removeProduct = new Product() });
        return response.Data.removeProduct;
    }
    #endregion

    #region Reservation
    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
                reservationQuery{
                    reservations{
                        id
                        name
                        phone
                        email
                        totalPeople
                        date
                        time
                    }
                }
            }"
        };
        var response = await _client.SendQueryAsync(request, () => new { reservations = new List<Reservation>() });
        return response.Data.reservations;
    }

    public async Task<Reservation> GetReservationById(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($id: Int){
                reservationQuery{
                    reservation(id: $id){
                        id
                        name
                        phone
                        email
                        totalPeople
                        date
                        time
                    } 
                }    
            }",
            Variables = new { id = id }
        };
        var response = await _client.SendQueryAsync(request, () => new { reservation = new Reservation() });
        return response.Data.reservation;
    }

    public async Task<Reservation> AddReservation(ReservationInput reservation)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($reservation: ReservationInputType)
            {
                reservationMutation{
                    addReservation(reservation: $reservation)
                    {
                        id
                        name
                        phone
                        email
                        totalPeople
                        date
                        time 
                    }
                }
            }",
            Variables = new { reservation = reservation }
        };
        var response = await _client.SendMutationAsync(request, () => new { addReservation = new Reservation() });
        return response.Data.addReservation;
    }

    public async Task<Reservation> UpdateReservation(int id, ReservationInput reservation)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $reservation: ReservationInputType){
                reservationMutation{
                    updateReservation(id: $id, reservation :$reservation){
                        id
                        name
                        phone
                        email
                        totalPeople
                        date
                        time
                    }
                }
            }",
            Variables = new { id = id, reservation = reservation }
        };
        var response = await _client.SendMutationAsync(request, () => new { updateReservation = new Reservation() });
        return response.Data.updateReservation;
    }

    public async Task<Reservation> DeleteReservation(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                reservationMutation{
                    removeReservation(id : $id){
                        id
                    }
                }
            }",
            Variables = new { id = id }
        };
        var response = await _client.SendMutationAsync(request, () => new { removeReservation = new Reservation() });
        return response.Data.removeReservation;
    }
    #endregion

}

