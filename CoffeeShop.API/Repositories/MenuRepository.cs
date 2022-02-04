using CoffeeShop.API.Data;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MenuRepository> _logger;

    public MenuRepository(AppDbContext dbContext, ILogger<MenuRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Menu> AddMenu(Menu menu)
    {
        try
        {
            _dbContext.Menus.Add(menu);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Menu added to DB.");
            return menu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not add Menu to DB: {ex.Message}");
            return null;
        }
    }

    public async Task<Menu> GetMenuById(int id)
    {
        try
        {
            var menu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id);
            _logger.LogInformation("Query 'menu' was successful.");
            return menu;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch menu with ID {id} from DB: {ex.Message}");
            return new Menu
            {
                Id = 0,
                Name = null,
                ImageUrl = null
            };
        }
    }

    public async Task<IEnumerable<Menu>> GetMenus()
    {
        try
        {
            var menus = await _dbContext.Menus.ToListAsync();
            _logger.LogInformation("Query 'menus' was successful.");
            return menus;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not fetch menus from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<IdModel> RemoveMenu(int id)
    {
        try
        {
            var menuObj = await GetMenuById(id);
            _dbContext.Menus.Remove(menuObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Menu with ID {id} removed from DB.");
            var idObj = new IdModel { Id = menuObj.Id };
            return idObj;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not remove Menu with ID {id} from DB: {ex.Message}");
            return null;
        }
    }

    public async Task<Menu> UpdateMenu(int id, Menu menu)
    {
        try
        {
            var menuObj = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id);
            menuObj.Name = menu.Name;
            menuObj.ImageUrl = menu.ImageUrl;
            _dbContext.Menus.Update(menuObj);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Menu with ID {id} updated in DB.");
            return menuObj;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not update menu with ID {id} in DB: {ex.Message}");
            return null;
        }

    }
}