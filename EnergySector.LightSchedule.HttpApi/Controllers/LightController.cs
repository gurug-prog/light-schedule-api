using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnergySector.LightSchedule.HttpApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LightController : ControllerBase
{
    private static readonly string[] Groups = new[]
    {
        "One", "Two", "Three", "Four", "Five", "Six"
    };

    private readonly ILogger<LightController> _logger;

    public LightController(ILogger<LightController> logger)
    {
        _logger = logger;
    }

    private TimeSpan ShiftTime(TimeSpan start, TimeSpan end)
    {
        int maxMinutes = (int)((end - start).TotalMinutes);
        return start.Add(TimeSpan.FromMinutes(Random.Shared.Next(maxMinutes)));
    }

    [HttpGet(Name = "GetLightSchedule")]
    public IEnumerable<LightSchedule> GetLightSchedule()
    {
        var start = TimeSpan.FromHours(0);
        var end = TimeSpan.FromHours(23);

        return Enumerable.Range(1, 5).Select(index => new LightSchedule
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TimeStart = (start = ShiftTime(start, end)),
            TimeFinish = ShiftTime(start, end),
            Group = Groups[Random.Shared.Next(Groups.Length)]
        })
        .ToArray();
    }
}
