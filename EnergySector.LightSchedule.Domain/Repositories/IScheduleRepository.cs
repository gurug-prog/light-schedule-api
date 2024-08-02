using EnergySector.LightSchedule.Domain.Entities;

namespace EnergySector.LightSchedule.Domain.Repositories;

public interface IScheduleRepository
{
    Task<bool> ImportSchedules(List<ScheduleEntity> schedules);
}
