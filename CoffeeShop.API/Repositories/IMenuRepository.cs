using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.Repositories;

public interface IMenuRepository
{
    Task<IEnumerable<Menu>> GetMenus();
    Task<Menu> GetMenuById(int id);
    Task<Menu> AddMenu(Menu menu);
    Task<Menu> UpdateMenu(int id, Menu menu);
    Task<IdModel> RemoveMenu(int id);
}