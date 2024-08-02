namespace EnergySector.LightSchedule.Application.Contracts;

public interface ILightGroupAppService
{
    Task<List<LightGroupDto>> ExportSchedules(
        IList<int>? groupIds = null,
        bool withSchedules = false,
        bool withAddresses = false);
    Task<LightGroupShutdownDto> GetGroupShutdown(int groupId);
    Task<bool> ImportSchedules(ScheduleFileUploadDto file);
    Task<LightGroupDto> UpdateGroupSchedules(
        int groupId,
        IList<ScheduleDto> schedules);
}
