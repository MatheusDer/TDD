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
    public async Task<IActionResult> BookRoom(RoomBookingRequest request)
    {
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var result = _processor.BookRoom(request);

		if (result.Flag == BookingFlag.Failure)
            return BadRequest("No Rooms Available");

		return Ok(result);
    }
}
