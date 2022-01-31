using CoffeeShop.Client.Clients;
using CoffeeShop.Client.Models.InputTypes;
using CoffeeShop.Client.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Client.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly GraphQLClient _client;
    private readonly IMessagePublisher _publisher;

    public ProductController(GraphQLClient client, IMessagePublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _client.GetProducts();
        return Ok(products);
    }

    [HttpGet("{menuId}")]
    public async Task<IActionResult> Get(int menuId)
    {
        var products = await _client.GetProductsForMenu(menuId);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductInput product)
    {
        var productObj = await _client.AddProduct(product);
        return Ok(productObj);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, ProductInput product)
    {
        var productObj = await _client.UpdateProduct(id, product);
        return Ok(productObj);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _client.DeleteProduct(id);
        return Ok(product);
    }
}