using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomBookingController : ControllerBase
{
    private readonly IRoomBookingRequestProcessor _processor;

    public RoomBookingController(IRoomBookingRequestProcessor processor)
    {
        _processor = processor;
    }

    [HttpPost]
    public IActionResult Test(RoomBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = _processor.BookRoom(request);

        if (result.Flag == BookingFlag.Failure)
            return NotFound("No Rooms Available");

        return Created("", result);
    }
}
