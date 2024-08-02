namespace EnergySector.LightSchedule.Application.Contracts;

public class LightGroupDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IList<AddressDto>? Addresses { get; set; }
    public IList<ScheduleDto>? Schedules { get; set; }
}
