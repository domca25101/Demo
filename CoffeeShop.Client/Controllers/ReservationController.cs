using CoffeeShop.Client.Clients;
using CoffeeShop.Client.Models.InputTypes;
using CoffeeShop.Client.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Client.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly GraphQLClient _client;
    private readonly IMessagePublisher _publisher;

    public ReservationController(GraphQLClient client, IMessagePublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

     [HttpGet]
    public async Task<IActionResult> Get()
    {
        var reservations = await _client.GetReservations();
         try
        {
            foreach (var reservation in reservations)
            {
                await _publisher.PublishReservation(reservation, "Publish");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var reservation = await _client.GetReservationById(id);
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ReservationInput reservation)
    {
        var reservationObj = await _client.AddReservation(reservation);
         try
        {
            await _publisher.PublishReservation(reservationObj, "Add");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(reservationObj);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, ReservationInput reservation)
    {
        var reservationObj = await _client.UpdateReservation(id, reservation);
         try
        {
            await _publisher.PublishReservation(reservationObj, "Update");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(reservationObj);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var str = await _client.DeleteReservation(id);
         try
        {
            await _publisher.Delete(id, "Reservation");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send: {ex.Message}");
        }
        return Ok(str);
    }
}