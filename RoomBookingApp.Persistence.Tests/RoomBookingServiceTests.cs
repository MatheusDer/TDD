using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Persistence.Repositories;
using Xunit;

namespace RoomBookingApp.Persistence;

public class RoomBookingServiceTests
{
    [Fact]
    public void Should_Return_Available_Rooms()
    {
        var date = DateTime.Now;

        var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase("InMem")
            .Options;

        using var context = new RoomBookingAppDbContext(dbOptions);

        context.Add(new Room { Id = 1, Name = "Room 1" });
        context.Add(new Room { Id = 2, Name = "Room 2" });
        context.Add(new Room { Id = 3, Name = "Room 3" });

        context.Add(new RoomBooking { RoomId = 1, Date = date });
        context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });

        context.SaveChanges();

        var roomBookingService = new RoomBookingService(context);

        var availableRooms = roomBookingService.GetAvailableRooms(date);

        Assert.Equal(2, availableRooms.Count());
        Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        Assert.Contains(availableRooms, r => r.Id == 2);
        Assert.Contains(availableRooms, r => r.Id == 3);
    }
}
