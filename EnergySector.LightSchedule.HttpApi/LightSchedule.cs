namespace EnergySector.LightSchedule.HttpApi.Controllers;

public class LightSchedule
{
    public DateOnly Date { get; set; }

    public TimeSpan TimeStart { get; set; }

    public TimeSpan TimeFinish { get; set; }

    public string? Group { get; set; }
}
