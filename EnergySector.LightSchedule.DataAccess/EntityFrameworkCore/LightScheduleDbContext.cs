using EnergySector.LightSchedule.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EnergySector.LightSchedule.DataAccess.EntityFrameworkCore;

public class LightScheduleDbContext : DbContext
{
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<LightGroupEntity> Groups { get; set; }
    public DbSet<ScheduleEntity> Schedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? throw new Exception("Database connection string is not configured"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
