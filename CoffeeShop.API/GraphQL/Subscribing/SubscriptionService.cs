using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscribing;

public class SubscriptionService
{
    private readonly ISubject<Menu> _menuStream = new ReplaySubject<Menu>(1);

    public Menu MenuAddedMessage(Menu menu)
    {
        _menuStream.OnNext(menu);
        return menu;
    }

    public IObservable<Menu> GetMessages()
    {
        return _menuStream.AsObservable();
    }

}