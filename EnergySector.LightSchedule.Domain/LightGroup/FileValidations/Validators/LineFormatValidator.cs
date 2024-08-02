using EnergySector.LightSchedule.Domain.Shared;

namespace EnergySector.LightSchedule.Domain.Schedule.Validators;

public class LineFormatValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        var parts = input.Split(new[] { '.', ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
        {
            throw new BusinessException($"Invalid line format.");
        }

        return parts;
    }
}
