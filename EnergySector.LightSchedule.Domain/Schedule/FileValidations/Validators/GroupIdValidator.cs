using EnergySector.LightSchedule.Domain.Shared;

namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public class GroupIdValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        if (!int.TryParse(input.Trim(), out var groupId))
        {
            throw new BusinessException($"Invalid group id value.");
        }

        return groupId;
    }
}
