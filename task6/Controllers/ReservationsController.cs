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
    
    /*
    [HttpGet]
    public IActionResult GetAll() => Ok(Reservations);
    */
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId
        )
    {
        var query = Reservations.AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.Date == date.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId);

        return Ok(query.ToList());
    }
    
    [HttpPost]
    public IActionResult Create(Reservation reservation)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var room = RoomsController.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room == null) return NotFound("Room does not exist.");
        if (!room.IsActive) return Conflict("Room is inactive.");

        if (reservation.EndTime <= reservation.StartTime)
            return BadRequest("EndTime must be later than StartTime.");

        bool overlap = Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime
        );

        if (overlap) return Conflict("Reservation overlaps.");

        reservation.Id = Reservations.Max(r => r.Id) + 1;
        Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Reservation updated)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        reservation.RoomId = updated.RoomId;
        reservation.OrganizerName = updated.OrganizerName;
        reservation.Topic = updated.Topic;
        reservation.Date = updated.Date;
        reservation.StartTime = updated.StartTime;
        reservation.EndTime = updated.EndTime;
        reservation.Status = updated.Status;

        return Ok(reservation);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        Reservations.Remove(reservation);
        return NoContent();
    }
}