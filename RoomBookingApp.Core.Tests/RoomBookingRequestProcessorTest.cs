using FluentAssertions;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using System.Diagnostics;
using Xunit;

namespace RoomBookingApp.Core;

public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestProcessor _processor;

    public RoomBookingRequestProcessorTest()
    {
        _processor = new RoomBookingRequestProcessor();
    }

    [Fact]
    public void BookRoom_ShouldReturnRoomBookingResponseWithRequestValues()
    {
        var bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@email.com",
            Date = new DateTime(2023, 1, 1)
        };

        RoomBookingResult result = _processor.BookRoom(bookingRequest);

        result.Should().NotBeNull();

        result.FullName.Should().Be(bookingRequest.FullName);
        result.Email.Should().Be(bookingRequest.Email);
        result.Date.Should().Be(bookingRequest.Date);
    }

    [Fact]
    public void BookRoom_ShouldThrow_WhenNullRequest()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
        exception.ParamName.Should().Be("bookingRequest");
    }
}
