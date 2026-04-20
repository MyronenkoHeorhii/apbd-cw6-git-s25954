using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task6.models;

namespace task6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    
    public static List<Reservation> Reservations = new List<Reservation>
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "organizer1",
            Topic = "topic1",
            Date = new DateOnly(2026,5,10),
            StartTime = new TimeOnly(9,59,59),
            EndTime = new TimeOnly(11,0,0),
            Status = "confirmed"
        },
        
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "organizer2",
            Topic = "topic2",
            Date = new DateOnly(2026,5,11),
            StartTime = new TimeOnly(10,5,5),
            EndTime = new TimeOnly(12,1,1),
            Status = "planned"
        }
    };
    
    [HttpGet]
    public IActionResult GetAll() => Ok(Reservations);
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
}