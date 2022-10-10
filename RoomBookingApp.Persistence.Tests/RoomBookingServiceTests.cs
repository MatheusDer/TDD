using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Persistence.Repositories;
using Xunit;

namespace RoomBookingApp.Persistence;

public class RoomBookingServiceTests
{
    private readonly DbContextOptions<RoomBookingAppDbContext> _dbOptions = 
        new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase("InMem")
            .Options;

    [Fact]
    public void Should_Return_Available_Rooms()
    {
        var date = DateTime.Now;

        using var context = new DatabaseFixture(_dbOptions);

        context.Add(new Room { Id = 1, Name = "Room 1" });
        context.Add(new Room { Id = 2, Name = "Room 2" });
        context.Add(new Room { Id = 3, Name = "Room 3" });

        context.Add(new RoomBooking { RoomId = 1, Date = date, FullName="Test1", Email="test1@email.com" });
        context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1), FullName = "Test2", Email = "test2@email.com" });

        context.SaveChanges();

        var roomBookingService = new RoomBookingService(context);

        var availableRooms = roomBookingService.GetAvailableRooms(date);

        Assert.Equal(2, availableRooms.Count());
        Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        Assert.Contains(availableRooms, r => r.Id == 2);
        Assert.Contains(availableRooms, r => r.Id == 3);
    }

    [Fact]
    public void Should_Save_RoomBooking()
    {
        var roomBooking = new RoomBooking 
        { 
            Date = DateTime.Now, 
            FullName = "Test", 
            Email = "test@email.com" ,
            RoomId = 1,
        };

        using var context = new DatabaseFixture(_dbOptions);

        var roomBookingService = new RoomBookingService(context);

        roomBookingService.Save(roomBooking);

        var roomBookings = context.RoomBookings.ToList();
        var savedRoomBooking = Assert.Single(roomBookings);

        savedRoomBooking.Should().NotBeNull();
        savedRoomBooking.Id.Should().Be(1);
        savedRoomBooking.Date.Should().Be(roomBooking.Date);
        savedRoomBooking.FullName.Should().Be(roomBooking.FullName);
        savedRoomBooking.Email.Should().Be(roomBooking.Email);
        savedRoomBooking.RoomId.Should().Be(roomBooking.RoomId);
    }
}
