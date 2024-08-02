using EnergySector.LightSchedule.DataAccess.EntityFrameworkCore;
using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.DataAccess.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly ILogger<ScheduleRepository> _logger;

    public ScheduleRepository(ILogger<ScheduleRepository> logger)
    {
        _logger = logger;
    }

    public async Task<bool> ImportSchedules(List<ScheduleEntity> schedules)
    {
        _logger.LogInformation("ScheduleRepository.ImportSchedules started");
        bool result = false;
        try
        {
            using var dbContext = new LightScheduleDbContext();
            await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Schedules;");
            await dbContext.Schedules.AddRangeAsync(schedules);
            await dbContext.SaveChangesAsync();

            result = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleRepository.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleRepository.ImportSchedules finished");
        }

        return result;
    }
}
