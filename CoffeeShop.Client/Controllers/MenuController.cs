using CoffeeShop.Client.Clients;
using CoffeeShop.Client.Models.InputTypes;
using CoffeeShop.Client.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Client.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly GraphQLClient _client;
    private readonly IMessagePublisher _publisher;
    private readonly SubscriptionClient _sClient;

    public MenuController(GraphQLClient client, IMessagePublisher publisher, SubscriptionClient sClient)
    {
        _client = client;
        _publisher = publisher;
        _sClient = sClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var menus = await _client.GetMenus();
        try
        {
            foreach (var menu in menus)
            {
                await _publisher.PublishMenu(menu, "Publish");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(menus);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var menu = await _client.GetMenuById(id);
        return Ok(menu);
    }

    [HttpPost]
    public async Task<IActionResult> Add(MenuInput menu)
    {
        var menuObj = await _client.AddMenu(menu);
        try
        {
            await _publisher.PublishMenu(menuObj, "Add");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(menuObj);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, MenuInput menu)
    {
        var menuObj = await _client.UpdateMenu(id, menu);
        try
        {
            await _publisher.PublishMenu(menuObj, "Update");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(menuObj);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var str = await _client.DeleteMenu(id);
         try
        {
            await _publisher.Delete(id, "Menu");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(str);
    }

}