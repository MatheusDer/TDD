using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors;

public class RoomBookingRequestProcessor
{
    private readonly IRoomBookingService _roomBookingService;

    public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
    {
        _roomBookingService = roomBookingService;
    }

    public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
    {
        if (bookingRequest is null)
            throw new ArgumentNullException(nameof(bookingRequest));

        var roomBookingResult = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);
        var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);

        if (!availableRooms.Any())
        {
            roomBookingResult.Flag = BookingFlag.Failure;
            return roomBookingResult;
        }

        var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
        roomBooking.RoomId = availableRooms.First().Id;

        _roomBookingService.Save(roomBooking);

        roomBookingResult.Flag = BookingFlag.Success;
        roomBookingResult.RoomBookingId = roomBooking.Id;

        return roomBookingResult;
    }

    private static T CreateRoomBookingObject<T>(RoomBookingRequest bookingRequest)
        where T : RoomBookingBase, new()
        => new()
        {
            FullName = bookingRequest.FullName,
            Email = bookingRequest.Email,
            Date = bookingRequest.Date
        };
}