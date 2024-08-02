using EnergySector.LightSchedule.Application.Contracts;

namespace EnergySector.LightSchedule.Application.Contracts;

public interface IScheduleAppService
{
    Task<bool> ImportSchedules(ScheduleFileUploadDto file);
    Task<List<LightGroupDto>> ExportSchedules(IList<int> groupIds, bool exportAll = false);

}
