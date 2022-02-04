using CoffeeShop.Client.Models;
using CoffeeShop.Client.Models.InputTypes;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.Clients;

public class ProductGQLClient
{
    private readonly IGraphQLClient _client;
    private readonly ILogger<ProductGQLClient> _logger;

    public ProductGQLClient(IGraphQLClient client, ILogger<ProductGQLClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
                products{
                    id
                    name
                    imageUrl
                    description
                    price
                    menuId
                    menu{ 
                        name
                        imageUrl
                    }
                }
            }"
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { products = new List<Product>() });
            _logger.LogInformation("Query request 'products' was successful.");
            return response.Data.products;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Query request 'products' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsForMenu(int menuId)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($menuId: Int){
                productsForMenu(menuId: $menuId){
                    id
                    name
                    imageUrl
                    description
                    price
                    menuId
                    menu{
                        name
                        imageUrl
                    }
                }   
            }",
            Variables = new { menuId = menuId }
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { productsForMenu = new List<Product>() });
            _logger.LogInformation("Query request 'productsForMenu' was successful.");
            return response.Data.productsForMenu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Query request 'productsForMenu' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Product> AddProduct(ProductInput product)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($product: ProductInputType){
                addProduct(product: $product){
                    id
                    name
                    description
                    price
                    imageUrl
                    menuId
                }
            }",
            Variables = new { product = product }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { addProduct = new Product() });
            _logger.LogInformation("Mutation Request 'addProduct' was successful.");
            return response.Data.addProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'addProduct' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Product> UpdateProduct(int id, ProductInput product)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $product: ProductInputType){
                updateProduct(id: $id, product :$product){
                    id
                    name
                    description
                    price
                    imageUrl
                    menuId
                }
            }",
            Variables = new { id = id, product = product }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { updateProduct = new Product() });
            _logger.LogInformation("Mutation request 'updateProduct' was successful");
            return response.Data.updateProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'updateProduct' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> DeleteProduct(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                removeProduct(id : $id) 
            }",
            Variables = new { id = id }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { removeProduct = new int?() });
            _logger.LogInformation("Mutation request 'removeProduct' was successful.");
            return response.Data.removeProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'removeProduct' was not successful: {ex.Message}");
            return null;
        }
    }
}