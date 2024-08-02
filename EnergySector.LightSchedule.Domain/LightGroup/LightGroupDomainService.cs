using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Repositories;
using EnergySector.LightSchedule.Domain.Schedule.FileValidations.Validators;
using EnergySector.LightSchedule.Domain.Schedule.Validators;
using EnergySector.LightSchedule.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.Domain.Schedule;

public class LightGroupDomainService
{
    private readonly ILogger<LightGroupDomainService> _logger;
    private readonly ILightGroupRepository _scheduleRepository;
    private readonly IDictionary<ValidationType, IScheduleFileValidator> _fileValidatorsMap;

    public LightGroupDomainService(
        ILogger<LightGroupDomainService> logger,
        ILightGroupRepository scheduleRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
        _fileValidatorsMap = new Dictionary<ValidationType, IScheduleFileValidator>()
        {
            { ValidationType.LineParts, new LineFormatValidator() },
            { ValidationType.ScheduleGroupId, new GroupIdValidator() },
            { ValidationType.DayOfWeek, new DayOfWeekValidator() },
            { ValidationType.TimeRange, new TimeRangeValidator() },
            { ValidationType.StartTime, new StartTimeValidator() },
            { ValidationType.FinishTime, new FinishTimeValidator() }
        };
    }


    public async Task<List<LightGroupEntity>> ExportSchedules(
        IList<int>? groupIds = null,
        bool withSchedules = false,
        bool withAddresses = false)
    {
        List<LightGroupEntity> result = [];
        _logger.LogInformation("LightGroupDomainService.ExportSchedules started");
        try
        {
            result = await _scheduleRepository
                .GetGroupList(groupIds, withSchedules, withAddresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupDomainService.ExportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupDomainService.ExportSchedules finished");
        }

        return result;
    }

    public async Task<LightGroupEntity> GetGroupById(
        int groupId,
        bool withSchedules = false,
        bool withAddresses = false)
    {
        LightGroupEntity result = new(0);
        _logger.LogInformation("LightGroupDomainService.ExportSchedules started");
        try
        {
            result = await _scheduleRepository.GetGroupById(
                groupId, withSchedules, withAddresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupDomainService.ExportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupDomainService.ExportSchedules finished");
        }

        return result;
    }

    public async Task<bool> ImportFile(string fileName, Stream readStream)
    {
        bool result = false;
        _logger.LogInformation("LightGroupDomainService.ImportFile started");
        try
        {
            const string permittedFileExtension = ".txt";
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || ext != permittedFileExtension)
            {
                throw new BusinessException($"Unsupported file extension: '{ext}'");
            }

            var schedules = await ParseFile(readStream);
            result = await _scheduleRepository.ImportSchedules(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupDomainService.ImportFile failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupDomainService.ImportFile finished");
        }

        return result;
    }

    private async Task<List<ScheduleEntity>> ParseFile(Stream stream)
    {
        List<ScheduleEntity> result = [];
        using var reader = new StreamReader(stream);
        var line = await reader.ReadLineAsync();
        var lineNumber = 1;
        try
        {
            while (line != null)
            {
                var parts = (string[])_fileValidatorsMap[ValidationType.LineParts].Validate(line);
                var groupId = (int)_fileValidatorsMap[ValidationType.ScheduleGroupId].Validate(parts[0]);
                var day = (string)_fileValidatorsMap[ValidationType.DayOfWeek].Validate(parts[1]);

                for (int i = 2; i < parts.Length; i++)
                {
                    var timeParts = (string[])_fileValidatorsMap[ValidationType.TimeRange].Validate(parts[i]);
                    var startTime = (TimeSpan)_fileValidatorsMap[ValidationType.StartTime].Validate(timeParts[0]);
                    var finishTime = (TimeSpan)_fileValidatorsMap[ValidationType.FinishTime].Validate(timeParts[1]);

                    result.Add(new ScheduleEntity(0)
                    {
                        Day = day,
                        StartTime = startTime,
                        FinishTime = finishTime,
                        GroupId = groupId
                    });
                }

                line = await reader.ReadLineAsync();
                lineNumber++;
            }
        }
        catch (Exception ex)
        {
            throw new BusinessException($"An error occurred while reading" +
                $"the file at line {lineNumber}: {ex.Message}");
        }

        return result;
    }

    public async Task<LightGroupEntity> UpdateGroupSchedules(
        int groupId,
        IList<ScheduleEntity> schedules)
    {
        LightGroupEntity result = new(0);
        _logger.LogInformation("LightGroupDomainService.UpdateGroupSchedules started");
        try
        {
            if (schedules.Count == 0)
            {
                throw new BusinessException("You have not provided schedules to update.");
            }

            result = await _scheduleRepository.UpdateGroupSchedules(groupId, schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LightGroupDomainService.UpdateGroupSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("LightGroupDomainService.UpdateGroupSchedules finished");
        }

        return result;
    }

}
