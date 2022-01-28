using CoffeeShop.API.Models;
using GraphQL.Types;

namespace CoffeeShop.API.GraphQL.Types;

public class ReservationType : ObjectGraphType<Reservation>
{
    public ReservationType()
    {
        Field(r => r.Id);
        Field(r => r.Name);
        Field(r => r.Phone);
        Field(r => r.Email);
        Field(r => r.TotalPeople);
        Field(r => r.Date);
        Field(r => r.Time);
    }
}