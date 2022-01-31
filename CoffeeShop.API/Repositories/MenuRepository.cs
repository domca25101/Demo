using CoffeeShop.API.Data;
using CoffeeShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _dbContext;

    public MenuRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Menu> AddMenu(Menu menu)
    {
        _dbContext.Menus.Add(menu);
        await _dbContext.SaveChangesAsync();
        return menu;
    }

    public async Task<Menu> GetMenuById(int id)
    {
        return await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Menu>> GetMenus()
    {
        return await _dbContext.Menus.ToListAsync();
    }

    public async Task<Menu> RemoveMenu(int id)
    {
        var menuObj = await GetMenuById(id);
        _dbContext.Menus.Remove(menuObj);
        await _dbContext.SaveChangesAsync();
        return menuObj;
    }

    public async Task<Menu> UpdateMenu(int id, Menu menu)
    {
        var menuObj = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == id);
        menuObj.Name = menu.Name;
        menuObj.ImageUrl = menu.ImageUrl;
        _dbContext.Menus.Update(menuObj);
        await _dbContext.SaveChangesAsync();
        return menuObj;
    }
}