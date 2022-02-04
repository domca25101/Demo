using CoffeeShop.Client.Models;
using CoffeeShop.Client.Models.InputTypes;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.Clients;

public class MenuGQLClient
{
    private readonly IGraphQLClient _client;
    private readonly ILogger<MenuGQLClient> _logger;

    public MenuGQLClient(IGraphQLClient client, ILogger<MenuGQLClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<Menu>> GetMenus()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
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
            }"
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { menus = new List<Menu>() });
            _logger.LogInformation("Query request 'menus' was successful.");
            return response.Data.menus;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Query request 'menus' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Menu> GetMenuById(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($id: Int){
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
            }",
            Variables = new { id = id }
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { menu = new Menu() });
            _logger.LogInformation("Query request 'menu' was successful");
            return response.Data.menu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Query request 'menu' was not successful: {ex.Message}");
            return new Menu
            {
                Id = 0,
                Name = "",
                ImageUrl = ""
            };
        }
    }

    public async Task<Menu> AddMenu(MenuInput menu)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($menu: MenuInputType)
            {
                addMenu(menu: $menu)
                {
                    id
                    name
                    imageUrl
                }
            }",
            Variables = new { menu = menu }
        };

        try
        {
            var response = await _client.SendMutationAsync(request, () => new { addMenu = new Menu() });
            _logger.LogInformation("Mutation request 'addMenu' was successful.");
            return response.Data.addMenu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'addMenu' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Menu> UpdateMenu(int id, MenuInput menu)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $menu: MenuInputType){
                updateMenu(id: $id, menu :$menu){
                    id
                    name
                    imageUrl
                }
            }",
            Variables = new { id = id, menu = menu }
        };

        try
        {
            var response = await _client.SendMutationAsync(request, () => new { updateMenu = new Menu() });
            _logger.LogInformation("Mutation Request 'updateMenu' was successful");
            return response.Data.updateMenu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'updateMenu' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> DeleteMenu(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                removeMenu(id : $id)
            }",
            Variables = new { id = id }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { removeMenu = new int?() });
            _logger.LogInformation("Mutation request 'removeMenu' was successful.");
            return response.Data.removeMenu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'removeMenu' was not successful: {ex.Message}");
            return null;
        }
    }
}