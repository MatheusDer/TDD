using FluentAssertions;
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using System.Diagnostics;
using Xunit;

namespace RoomBookingApp.Core;

public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestProcessor _processor;
    private readonly RoomBookingRequest _bookingRequest;
    private readonly Mock<IRoomBookingService> _roomBookingServiceMock;

    public RoomBookingRequestProcessorTest()
    {

        _bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@email.com",
            Date = new DateTime(2023, 1, 1)
        };

        _roomBookingServiceMock = new Mock<IRoomBookingService>();
        _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
    }

    [Fact]
    public void BookRoom_ShouldReturnRoomBookingResponseWithRequestValues()
    {
        RoomBookingResult result = _processor.BookRoom(_bookingRequest);

        result.Should().NotBeNull();

        result.FullName.Should().Be(_bookingRequest.FullName);
        result.Email.Should().Be(_bookingRequest.Email);
        result.Date.Should().Be(_bookingRequest.Date);
    }

    [Fact]
    public void BookRoom_ShouldThrow_WhenNullRequest()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
        exception.ParamName.Should().Be("bookingRequest");
    }

    [Fact]
    public void Sould_Save_RoomBooking_Request()
    {
        RoomBooking savedRoomBooking = null;
        _roomBookingServiceMock.Setup(r => r.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>((booking) => savedRoomBooking = booking);

        _processor.BookRoom(_bookingRequest);

        _roomBookingServiceMock.Verify(r => r.Save(It.IsAny<RoomBooking>()), Times.Once);
        savedRoomBooking.Should().NotBeNull();
        
        _bookingRequest.FullName.Should().Be(savedRoomBooking.FullName);
        _bookingRequest.Email.Should().Be(savedRoomBooking.Email);
        _bookingRequest.Date.Should().Be(savedRoomBooking.Date);
    }
}
