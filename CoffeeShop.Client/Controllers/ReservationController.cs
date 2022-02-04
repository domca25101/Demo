using CoffeeShop.Client.Clients;
using CoffeeShop.Client.Models.InputTypes;
using CoffeeShop.Client.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Client.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly ReservationGQLClient _client;

    public ReservationController(ReservationGQLClient client)
    {
        _client = client;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var reservations = await _client.GetReservations();
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
        return Ok(reservationObj);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, ReservationInput reservation)
    {
        var reservationObj = await _client.UpdateReservation(id, reservation);
        return Ok(reservationObj);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _client.DeleteReservation(id);
        return Ok(reservation);
    }
}