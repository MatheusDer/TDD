using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
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

            _roomBookingService.Save(CreateRoomBookingObject<RoomBooking>(bookingRequest));

            return CreateRoomBookingObject<RoomBookingResult>(bookingRequest);
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
}