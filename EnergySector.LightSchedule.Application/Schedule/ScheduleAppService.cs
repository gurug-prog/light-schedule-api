﻿using AutoMapper;
using EnergySector.LightSchedule.Application.Contracts;
using EnergySector.LightSchedule.Domain.Entities;
using EnergySector.LightSchedule.Domain.Schedule;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.Application.Schedule;

public class ScheduleAppService : IScheduleAppService
{
    private readonly ILogger<ScheduleAppService> _logger;
    private readonly IMapper _mapper;
    private readonly ScheduleDomainService _scheduleDomainService;

    public ScheduleAppService(
        ILogger<ScheduleAppService> logger,
        IMapper mapper,
        ScheduleDomainService scheduleDomainService)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleDomainService = scheduleDomainService;
    }

    public async Task<List<LightGroupDto>> ExportSchedules(
        IList<int>? groupIds = null,
        bool withSchedules = false,
        bool withAddresses = false)
    {
        List<LightGroupDto> result = [];
        _logger.LogInformation("ScheduleAppService.ImportSchedules started");
        try
        {
            var entities = await _scheduleDomainService
                .ExportSchedules(groupIds, withSchedules, withAddresses);
            result = _mapper.Map<List<LightGroupEntity>, List<LightGroupDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.ImportSchedules finished");
        }

        return result;
    }

    public async Task<LightGroupShutdownDto> GetGroupShutdown(int groupId)
    {
        LightGroupShutdownDto result = new();
        _logger.LogInformation("ScheduleAppService.GetGroupShutdown started");
        try
        {
            var groupEntity = await _scheduleDomainService
                .GetGroupById(groupId, withSchedules: true);
            if (groupEntity.Schedules is null || groupEntity.Schedules.Count == 0)
            {
                return new LightGroupShutdownDto()
                {
                    IsShutdown = null,
                    Message = "The status is unknown. Not found any schedules " +
                        $"configured for group with id {groupId}"
                };
            }

            var today = DateTime.Now;
            var dayOfWeek = today.ToString("dddd").ToLowerInvariant();
            var effectiveSchedule = groupEntity.Schedules
                .Where(s => s.Day.ToLowerInvariant() == dayOfWeek
                    && s.StartTime <= today.TimeOfDay
                    && today.TimeOfDay <= s.FinishTime)
                .FirstOrDefault();
            var isShutdown = effectiveSchedule is not null;

            result = new LightGroupShutdownDto()
            {
                IsShutdown = isShutdown,
                Message = isShutdown
                    ? $"The group with id {groupId} has a blackout now. " +
                        $"Electricity will be restored at approximately {effectiveSchedule!.FinishTime}"
                    : $"No shutdowns have been found by group with id {groupId}. " +
                        $"If you have a light shutdown in your local area, there is " +
                        $"probably an emergency case.",
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.GetGroupShutdown failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.GetGroupShutdown finished");
        }

        return result;
    }

    public async Task<bool> ImportSchedules(ScheduleFileUploadDto file)
    {
        bool result = false;
        _logger.LogInformation("ScheduleAppService.ImportSchedules started");
        try
        {
            result = await _scheduleDomainService.ImportFile(file.FileName, file.ReadStream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.ImportSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.ImportSchedules finished");
        }

        return result;
    }

    public async Task<LightGroupDto> UpdateGroupSchedules(
        int groupId,
        IList<ScheduleDto> schedules)
    {
        LightGroupDto result = new();
        _logger.LogInformation("ScheduleAppService.UpdateGroupSchedules started");
        try
        {
            var scheduleEntities = _mapper.Map<IList<ScheduleDto>, IList<ScheduleEntity>>(schedules);
            var group = await _scheduleDomainService.UpdateGroupSchedules(groupId, scheduleEntities);
            result = _mapper.Map<LightGroupEntity, LightGroupDto>(group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ScheduleAppService.UpdateGroupSchedules failed");
            throw;
        }
        finally
        {
            _logger.LogInformation("ScheduleAppService.UpdateGroupSchedules finished");
        }

        return result;
    }
}
