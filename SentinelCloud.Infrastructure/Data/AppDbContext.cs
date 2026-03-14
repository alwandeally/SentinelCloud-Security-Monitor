using Microsoft.EntityFrameworkCore;
using SentinelCloud.Core.Entities;

namespace SentinelCloud.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<LogEvent> LogEvents => Set<LogEvent>();
    public DbSet<SecurityAlert> SecurityAlerts => Set<SecurityAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LogEvent>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.IpAddress)
                .HasMaxLength(50);

            entity.Property(x => x.Username)
                .HasMaxLength(100);

            entity.Property(x => x.EventType)
                .HasMaxLength(50);

            entity.Property(x => x.Status)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<SecurityAlert>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasMaxLength(1000);

            entity.Property(x => x.IpAddress)
                .HasMaxLength(50);
        });
    }
}
