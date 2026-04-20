using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task6.models;

namespace task6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    public static List<Room> Rooms = new List<Room>
    {
        new Room
        {
            Id = 1,
            Name = "RoomName1",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 4,
            HasProjector = true,
            IsActive = true
        },
        
        new Room 
            { Id = 2,
                Name = "RoomName2",
                BuildingCode = "A",
                Floor = 2,
                Capacity = 3,
                HasProjector = false,
                IsActive = false 
            },
        
        new Room 
            { 
                Id = 3,
                Name = "RoomName3",
                BuildingCode = "B",
                Floor = 1,
                Capacity = 2,
                HasProjector = true,
                IsActive = true 
            },
        
        new Room
        {
            Id = 4,
            Name = "RoomName4",
            BuildingCode = "B",
            Floor = 3,
            Capacity = 1,
            HasProjector = false,
            IsActive = false
        }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Rooms);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }
}