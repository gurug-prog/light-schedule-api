using EnergySector.LightSchedule.Domain.Entities;

namespace EnergySector.LightSchedule.Domain.Repositories;

public interface ILightGroupRepository
{
    Task<LightGroupEntity> GetGroupById(
        int groupId,
        bool withSchedules = false,
        bool withAddresses = false);
    Task<List<LightGroupEntity>> GetGroupList(
        IList<int>? groupIds = null,
        bool withSchedules = false,
        bool withAddresses = false);
    Task<bool> ImportSchedules(List<ScheduleEntity> schedules);
    Task<LightGroupEntity> UpdateGroupSchedules(
        int groupId,
        IList<ScheduleEntity> schedules);
}
