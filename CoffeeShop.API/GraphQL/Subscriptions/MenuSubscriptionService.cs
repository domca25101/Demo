using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class MenuSubscriptionService
{
    private readonly ISubject<Menu> _addMenuStream = new ReplaySubject<Menu>(1);
    private readonly ISubject<Menu> _updateMenuStream = new ReplaySubject<Menu>(1);
    private readonly ISubject<Menu> _removeMenuStream = new ReplaySubject<Menu>(1);

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

    public Menu MenuDeleted(Menu menu)
    {
        _removeMenuStream.OnNext(menu);
        return menu;
    }

    public IObservable<Menu> GetAddedMenu()
    {
        return _addMenuStream.AsObservable();
    }

    public IObservable<Menu> GetUpdatedMenu()
    {
        return _updateMenuStream.AsObservable();
    }

    public IObservable<Menu> GetRemovedMenu()
    {
        return _removeMenuStream.AsObservable();
    }
}