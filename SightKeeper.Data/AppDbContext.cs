using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
	public DbSet<ModelConfig> ModelConfigs { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured) return;
		SetupLogging(optionsBuilder);
		SetupSqlite(optionsBuilder);
	}

	private static void SetupLogging(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.LogTo(content => Log.Verbose("[EF] {Content}", content), LogLevel.Trace);
		optionsBuilder.EnableSensitiveDataLogging();
	}

	private static void SetupSqlite(DbContextOptionsBuilder optionsBuilder)
	{
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = "App.db"
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Entity>().Property<int>("Id");
		modelBuilder.Entity<Entity>().HasKey("Id");
		modelBuilder.Entity<Model>().OwnsOne(model => model.Resolution);
		modelBuilder.Entity<DetectorItem>().OwnsOne(item => item.BoundingBox);
		modelBuilder.Entity<DetectorModel>().HasMany(model => model.Assets).WithOne().IsRequired();
		modelBuilder.Entity<DetectorAsset>().HasOne(asset => asset.Screenshot).WithOne().HasPrincipalKey<DetectorAsset>("Id").IsRequired();
		modelBuilder.Entity<Model>().HasMany(model => model.ItemClasses).WithOne().IsRequired();
	}
}