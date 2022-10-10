using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using System.Linq;

namespace RoomBookingApp.Persistence.Repositories;

public class RoomBookingService : IRoomBookingService
{
    private readonly RoomBookingAppDbContext _context;

    public RoomBookingService(RoomBookingAppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Room> GetAvailableRooms(DateTime date)
        => _context.Rooms.Where(r => !r.RoomBookings.Any() || r.RoomBookings.Any(r => r.Date < date)).ToList();

    public void Save(RoomBooking roomBooking)
    {
        _context.RoomBookings.Add(roomBooking);
        _context.SaveChanges();
    }
}
