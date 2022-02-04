using CoffeeShop.Client.Models;
using CoffeeShop.Client.Models.InputTypes;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CoffeeShop.Client.Clients;

public class ReservationGQLClient
{
    private readonly IGraphQLClient _client;
    private readonly ILogger<ReservationGQLClient> _logger;

    public ReservationGQLClient(IGraphQLClient client, ILogger<ReservationGQLClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query{
                reservations{
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time
                }
            }"
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { reservations = new List<Reservation>() });
            _logger.LogInformation("Query request 'reservations' was successfyl.");
            return response.Data.reservations;
        }
        catch (Exception ex)
        {
            _logger.LogError($"query request 'reservations' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Reservation> GetReservationById(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            query($id: Int){
                reservation(id: $id){
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time
                }   
            }",
            Variables = new { id = id }
        };
        try
        {
            var response = await _client.SendQueryAsync(request, () => new { reservation = new Reservation() });
            _logger.LogInformation("Query request 'reservation' was successful.");
            return response.Data.reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Query request 'reservation' was not successful: {ex.Message}");
            return new Reservation
            {
                Id = 0,
                Name = "",
                Phone = "",
                Email = "",
                TotalPeople = 0,
                Date = "",
                Time = ""
            };
        }
    }

    public async Task<Reservation> AddReservation(ReservationInput reservation)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($reservation: ReservationInputType){
                addReservation(reservation: $reservation)
                {
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time 
                }
            }",
            Variables = new { reservation = reservation }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { addReservation = new Reservation() });
            _logger.LogInformation("Mutation request 'addReservation' was successful.");
            return response.Data.addReservation;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'addReservation' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<Reservation> UpdateReservation(int id, ReservationInput reservation)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id: Int, $reservation: ReservationInputType){
                updateReservation(id: $id, reservation :$reservation){
                    id
                    name
                    phone
                    email
                    totalPeople
                    date
                    time
                }
            }",
            Variables = new { id = id, reservation = reservation }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { updateReservation = new Reservation() });
            _logger.LogInformation("Mutation request 'updateResevation' was successful.");
            return response.Data.updateReservation;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Mutation request 'updateReservation' was not successful: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> DeleteReservation(int id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = @"
            mutation($id : Int){
                removeReservation(id : $id)
            }",
            Variables = new { id = id }
        };
        try
        {
            var response = await _client.SendMutationAsync(request, () => new { removeReservation = new int?() });
            _logger.LogInformation("Mutation request 'removeReservation' was successful.");
            return response.Data.removeReservation;
        }
        catch (System.Exception)
        {
            _logger.LogError($"Mutation request 'removeReservation' was not successful.");
            return null;
        }
    }
}