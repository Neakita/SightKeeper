using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using SightKeeper.Data.Configuration;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
		Log.Debug("Instantiated {Name}", nameof(AppDbContext));
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
		Log.Debug("Instantiated {Name} with options {@Options}", nameof(AppDbContext), options);
	}

	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<DataSet> DataSets { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured) return;
		SetupLogging(optionsBuilder);
		SetupSqlite(optionsBuilder);
	}

	public override void Dispose()
	{
		Log.Debug("Disposing {Name}", nameof(AppDbContext));
		base.Dispose();
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
		modelBuilder.ApplyConfiguration(new GenericAssetConfiguration<DetectorAsset>());
		modelBuilder.ApplyConfiguration(new DetectorDataSetConfiguration());
		modelBuilder.ApplyConfiguration(new ScreenshotsLibraryConfiguration());
		modelBuilder.ApplyConfiguration(new ScreenshotConfiguration());
		modelBuilder.ApplyConfiguration(new AssetConfiguration());
		modelBuilder.ApplyConfiguration(new ItemClassConfiguration());
		modelBuilder.ApplyConfiguration(new GameConfiguration());
		modelBuilder.ApplyConfiguration(new DetectorItemConfiguration());
		modelBuilder.ApplyConfiguration(new ImageConfiguration());
		modelBuilder.ApplyConfiguration(new ScreenshotImageConfiguration());
		modelBuilder.ApplyConfiguration(new WeightsConfiguration());
		modelBuilder.ApplyConfiguration(new DataSetConfiguration());
		modelBuilder.ApplyConfiguration(new WeightsLibraryConfiguration());
		modelBuilder.Entity<Profile>().HasShadowKey();
		
		modelBuilder.Entity<Screenshot>().Navigation(screenshot => screenshot.Asset).AutoInclude();
		modelBuilder.Entity<Asset>().Navigation(asset => asset.Screenshot).AutoInclude();
	}
}