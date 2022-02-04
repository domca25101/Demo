using CoffeeShop.Client.Clients;
using CoffeeShop.Client.Models.InputTypes;
using CoffeeShop.Client.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Client.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly MenuGQLClient _client;

    public MenuController(MenuGQLClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var menus = await _client.GetMenus();
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
        return Ok(menuObj);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, MenuInput menu)
    {
        var menuObj = await _client.UpdateMenu(id, menu);
        return Ok(menuObj);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var menu = await _client.DeleteMenu(id);
        return Ok(menu);
    }

}