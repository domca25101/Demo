using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class ProductSubscriptionService
{
    private readonly ISubject<Product> _addProductStream = new Subject<Product>();
    private readonly ISubject<Product> _updateProductStream = new Subject<Product>();
    private readonly ISubject<IdModel> _removeProductStream = new Subject<IdModel>();

    public Product ProductAdded(Product product)
    {
        _addProductStream.OnNext(product);
        return product;
    }

    public Product ProductUpdated(Product product)
    {
        _updateProductStream.OnNext(product);
        return product;
    }

    public IdModel ProductDeleted(IdModel id)
    {
        _removeProductStream.OnNext(id);
        return id;
    }

    public IObservable<Product> GetAddedProduct()
    {
        return _addProductStream.AsObservable();
    }

    public IObservable<Product> GetUpdatedProduct()
    {
        return _updateProductStream.AsObservable();
    }

    public IObservable<IdModel> GetRemovedProduct()
    {
        return _removeProductStream.AsObservable();
    }
}