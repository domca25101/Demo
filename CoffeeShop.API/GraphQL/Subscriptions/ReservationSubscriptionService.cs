using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class ReservationSubscriptionService
{
    private readonly ISubject<Reservation> _addReservationStream = new Subject<Reservation>();
    private readonly ISubject<Reservation> _updateReservationStream = new Subject<Reservation>();
    private readonly ISubject<IdModel> _removeReservationStream = new Subject<IdModel>();

    public Reservation ReservationAdded(Reservation reservation)
    {
        _addReservationStream.OnNext(reservation);
        return reservation;
    }

    public Reservation ReservationUpdated(Reservation reservation)
    {
        _updateReservationStream.OnNext(reservation);
        return reservation;
    }

    public IdModel ReservationDeleted(IdModel id)
    {
        _removeReservationStream.OnNext(id);
        return id;
    }

    public IObservable<Reservation> GetAddedReservation()
    {
        return _addReservationStream.AsObservable();
    }

    public IObservable<Reservation> GetUpdatedReservation()
    {
        return _updateReservationStream.AsObservable();
    }

    public IObservable<IdModel> GetRemovedReservation()
    {
        return _removeReservationStream.AsObservable();
    }
}