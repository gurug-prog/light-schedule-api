namespace EnergySector.LightSchedule.Application.Contracts;

public class ScheduleFileUploadDto
{
    public required string FileName { get; set; }
    public required Stream ReadStream { get; set; }
}
