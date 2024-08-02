namespace EnergySector.LightSchedule.Application.Contracts;

public class AddressDto
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public int? GroupId { get; set; }
}
