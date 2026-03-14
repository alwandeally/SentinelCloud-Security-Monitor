using Microsoft.EntityFrameworkCore;
using SentinelCloud.Core.Interfaces;
using SentinelCloud.Infrastructure.Data;
using SentinelCloud.Infrastructure.Parsing;
using SentinelCloud.Infrastructure.Services;

namespace SentinelCloud.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ILogParser, CsvLogParser>();
        builder.Services.AddScoped<IThreatDetectionService, ThreatDetectionService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}