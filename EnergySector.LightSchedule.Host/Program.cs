using EnergySector.LightSchedule.DataAccess.EntityFrameworkCore;
using EnergySector.LightSchedule.Host.Config;
using EnergySector.LightSchedule.Host.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace EnergySector.LightSchedule.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application configuration.
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        using (var dbContext = new LightScheduleDbContext())
        {
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.Run();
    }
}
