using Microsoft.EntityFrameworkCore;

namespace RoomBookingApp.Persistence;

public class DatabaseFixture : RoomBookingAppDbContext, IDisposable
{
    private readonly DbContextOptions<RoomBookingAppDbContext> options;

    public DatabaseFixture(DbContextOptions<RoomBookingAppDbContext> options) 
        : base(options)
    {
        this.options = options;
    }

    public new void Dispose()
    {
        var context = new RoomBookingAppDbContext(options);
        context.Database.EnsureDeleted();

        base.Dispose();
    }
}
