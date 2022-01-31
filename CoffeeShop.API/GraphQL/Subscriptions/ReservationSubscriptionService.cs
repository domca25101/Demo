using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.GraphQL.Subscriptions;

public class ReservationSubscriptionService
{
    private readonly ISubject<Reservation> _addReservationStream = new Subject<Reservation>();
    private readonly ISubject<Reservation> _updateReservationStream = new Subject<Reservation>();
    private readonly ISubject<Reservation> _removeReservationStream = new ReplaySubject<Reservation>();

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

    public Reservation ReservationDeleted(Reservation reservation)
    {
        _removeReservationStream.OnNext(reservation);
        return reservation;
    }

    public IObservable<Reservation> GetAddedReservation()
    {
        return _addReservationStream.AsObservable();
    }

    public IObservable<Reservation> GetUpdatedReservation()
    {
        return _updateReservationStream.AsObservable();
    }

    public IObservable<Reservation> GetRemovedReservation()
    {
        return _removeReservationStream.AsObservable();
    }
}