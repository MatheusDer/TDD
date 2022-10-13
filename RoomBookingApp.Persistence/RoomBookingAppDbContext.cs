using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Domain;

namespace RoomBookingApp.Persistence;

public class RoomBookingAppDbContext : DbContext
{
	public RoomBookingAppDbContext(DbContextOptions<RoomBookingAppDbContext> options)
		: base(options) { }

	public DbSet<Room> Rooms { get; set; }
	public DbSet<RoomBooking> RoomBookings { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Room>().HasData(new Room { Id = 1, Name = "Conference Room A" });
    }
}
