using EnergySector.LightSchedule.DataAccess.EntityFrameworkCore;
using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Repositories;
using EnergySector.LightSchedule.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.DataAccess.Repositories;

public class LightGroupRepository : ILightGroupRepository
{
    private readonly ILogger<LightGroupRepository> _logger;

    public LightGroupRepository(ILogger<LightGroupRepository> logger)
    {
        _logger = logger;
    }

    public async Task<LightGroupEntity> GetGroupById(
        int groupId,
        bool withSchedules = false,
        bool withAddresses = false)
    {
        _logger.LogInformation("LightGroupRepository.GetGroupById started");
        LightGroupEntity result = new(0);
        try
        {
            using var dbContext = new LightScheduleDbContext();
            IQueryable<LightGroupEntity> query = dbContext.Groups
                .Where(g => g.Id == groupId)
                .IncludeIf(withSchedules, x => x.Schedules)
                .IncludeIf(withAddresses, x => x.Addresses);
            result = await query.FirstOrDefaultAsync()
                ?? throw new BusinessException($"The light group with id {groupId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupRepository.GetGroupById failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupRepository.GetGroupById finished");
        }

        return result;
    }

    public async Task<List<LightGroupEntity>> GetGroupList(
        IList<int>? groupIds = null,
        bool withSchedules = false,
        bool withAddresses = false)
    {
        _logger.LogInformation("LightGroupRepository.GetList started");
        List<LightGroupEntity> result = [];
        try
        {
            using var dbContext = new LightScheduleDbContext();
            IQueryable<LightGroupEntity> query = dbContext.Groups
                .WhereIf(groupIds is not null, g => groupIds!.Contains(g.Id))
                .IncludeIf(withSchedules, x => x.Schedules)
                .IncludeIf(withAddresses, x => x.Addresses);
            result = await query.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupRepository.GetList failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupRepository.GetList finished");
        }

        return result;
    }

    public async Task<bool> ImportSchedules(List<ScheduleEntity> schedules)
    {
        _logger.LogInformation("LightGroupRepository.ImportSchedules started");
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
            _logger.LogError(ex, "LightGroupRepository.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupRepository.ImportSchedules finished");
        }

        return result;
    }

    public async Task<LightGroupEntity> UpdateGroupSchedules(
        int groupId,
        IList<ScheduleEntity> schedules)
    {
        _logger.LogInformation("LightGroupRepository.UpdateGroupSchedules started");
        LightGroupEntity result = new(0);
        try
        {
            using var dbContext = new LightScheduleDbContext();
            var query = dbContext.Groups
                .Where(g => g.Id == groupId)
                .Include(g => g.Schedules);
            var group = await query.FirstOrDefaultAsync()
                ?? throw new BusinessException($"The light group with id {groupId} not found.");

            if (group.Schedules is null || group.Schedules.Count == 0)
            {
                throw new BusinessException(
                    $"There is no registered schedules for group with id {groupId}.");
            }

            foreach (var updSchedule in schedules)
            {
                var schedule = group.Schedules.FirstOrDefault(s => s.Id == updSchedule.Id);
                if (schedule is not null)
                {
                    schedule.Day = updSchedule.Day;
                    schedule.StartTime = updSchedule.StartTime;
                    schedule.FinishTime = updSchedule.FinishTime;
                }
                else
                {
                    throw new BusinessException(
                        $"The schedule with id {updSchedule.Id} does not exist.");
                }
            }

            await dbContext.SaveChangesAsync();
            result = group;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupRepository.UpdateGroupSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupRepository.UpdateGroupSchedules finished");
        }

        return result;
    }
}
