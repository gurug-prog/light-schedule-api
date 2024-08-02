using EnergySector.LightSchedule.Domain.Shared;

namespace EnergySector.LightSchedule.Domain.Schedule.FileValidations.Validators;

public class DayOfWeekValidator : IScheduleFileValidator
{
    public object Validate(string input)
    {
        var dayOfWeek = input.Trim();
        var daysPossible = Enum.GetNames<DayOfWeek>().Select(x => x.ToLowerInvariant());

        if (!daysPossible.Contains(dayOfWeek.ToLowerInvariant()))
        {
            throw new BusinessException("Invalid day of week provided.");
        }

        return dayOfWeek;
    }
}
