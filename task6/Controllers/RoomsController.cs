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
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var query = Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }
    
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }
    
    [HttpPost]
    public IActionResult Create(Room room)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        room.Id = Rooms.Max(r => r.Id) + 1;
        Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Room updated)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var room = Rooms.FirstOrDefault(r => r.Id == id);
        
        if (room == null)
        {
            return NotFound();
        }

        room.Name = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor = updated.Floor;
        room.Capacity = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive = updated.IsActive;

        return Ok(room);
    }
}