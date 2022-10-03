using FluentAssertions;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Xunit;

namespace RoomBookingApp.Core;

public class RoomBookingRequestProcessorTest
{
    [Fact]
    public void BookRoom_ShouldReturnRoomBookingResponseWithRequestValues()
    {
        var bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@email.com",
            Date = new DateTime(2023, 1, 1)
        };

        var processor = new RoomBookingRequestProcessor();

        RoomBookingResult result = processor.BookRoom(bookingRequest);

        result.Should().NotBeNull();

        result.FullName.Should().Be(bookingRequest.FullName);
        result.Email.Should().Be(bookingRequest.Email);
        result.Date.Should().Be(bookingRequest.Date);
    }
}
