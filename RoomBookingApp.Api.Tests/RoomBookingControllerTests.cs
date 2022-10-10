using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Xunit;

namespace RoomBookingApp.Api;

public class RoomBookingControllerTests
{
	private Mock<IRoomBookingRequestProcessor> _roomBookingProcessor;
	private RoomBookingController _controller;
	private RoomBookingRequest _request;
	private RoomBookingResult _result;

	public RoomBookingControllerTests()
	{
		_roomBookingProcessor = new Mock<IRoomBookingRequestProcessor>();
		_controller = new RoomBookingController(_roomBookingProcessor.Object);
		_request = new RoomBookingRequest();
		_result = new RoomBookingResult();

        _roomBookingProcessor.Setup(r => r.BookRoom(_request))
			.Returns(_result);
	}

	[Theory]
	[InlineData(1, true, typeof(OkObjectResult))]
	[InlineData(0, false, typeof(BadRequestObjectResult))]
	public async Task Should_Call_RoomBooking_Method_When_Valid
	(
		int expecthedMethodCalls, 
		bool isModelValid, 
		Type expectedActionResultType
	)
	{
		if (!isModelValid)
			_controller.ModelState.AddModelError("Key", "ErrorMessage");

		var result = await _controller.BookRoom(_request);

		result.Should().BeOfType(expectedActionResultType);
        _roomBookingProcessor.Verify(r => r.BookRoom(_request), Times.Exactly(expecthedMethodCalls));
	}
}
