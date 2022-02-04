using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class MenuSubscriptionService
{
    private readonly ISubject<Menu> _addMenuStream = new Subject<Menu>();
    private readonly ISubject<Menu> _updateMenuStream = new Subject<Menu>();
    private readonly ISubject<IdModel> _removeMenuStream = new Subject<IdModel>();

    public Menu MenuAdded(Menu menu)
    {
        _addMenuStream.OnNext(menu);
        return menu;
    }

    public Menu MenuUpdated(Menu menu)
    {
        _updateMenuStream.OnNext(menu);
        return menu;
    }

    public IdModel MenuDeleted(IdModel id)
    {
        _removeMenuStream.OnNext(id);
        return id;
    }

    public IObservable<Menu> GetAddedMenu()
    {
        return _addMenuStream.AsObservable();
    }

    public IObservable<Menu> GetUpdatedMenu()
    {
        return _updateMenuStream.AsObservable();
    }

    public IObservable<IdModel> GetRemovedMenu()
    {
        return _removeMenuStream.AsObservable();
    }
}