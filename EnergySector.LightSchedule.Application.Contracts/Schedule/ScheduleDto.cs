namespace EnergySector.LightSchedule.Application.Contracts;

public class ScheduleDto
{
    public int Id { get; set; }
    public required string Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int GroupId { get; set; }
}
