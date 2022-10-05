using FluentAssertions;
using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
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
    private readonly List<Room> _availableRooms;

    public RoomBookingRequestProcessorTest()
    {

        _bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@email.com",
            Date = new DateTime(2023, 1, 1)
        };

        _availableRooms = new List<Room>() { new Room() { Id = 1} };

        _roomBookingServiceMock = new Mock<IRoomBookingService>();
        _roomBookingServiceMock.Setup(r => r.GetAvailableRooms(_bookingRequest.Date))
            .Returns(_availableRooms);
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

        savedRoomBooking.RoomId.Should().Be(_availableRooms.First().Id);
    }

    [Fact]
    public void Sould_Not_Save_RoomBooking_Request_If_None_Available()
    {
        _availableRooms.Clear();

        _processor.BookRoom(_bookingRequest);

        _roomBookingServiceMock.Verify(r => r.Save(It.IsAny<RoomBooking>()), Times.Never);
    }

    [Theory]
    [InlineData(BookingFlag.Success, true)]
    [InlineData(BookingFlag.Failure, false)]
    public void Should_Return_Flag_In_Result(BookingFlag bookingFlag, bool isAvailable)
    {
        if (!isAvailable)
            _availableRooms.Clear();

        var result = _processor.BookRoom(_bookingRequest);

        result.Flag.Should().Be(bookingFlag);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(null, false)]
    public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
    {
        if (!isAvailable)
            _availableRooms.Clear();
        else
            _roomBookingServiceMock.Setup(r => r.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>((booking) => booking.Id = roomBookingId.Value);


        var result = _processor.BookRoom(_bookingRequest);

        result.RoomBookingId.Should().Be(roomBookingId);
    }
}
