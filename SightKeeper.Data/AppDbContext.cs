using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using SightKeeper.Data.Configuration;
using SightKeeper.Domain;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data;

public class AppDbContext : DbContext
{
	public DbSet<DataSet> DataSets { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<Image> Images { get; set; } = null!;

	public AppDbContext()
	{
		Log.Debug("Instantiated {Name}", nameof(AppDbContext));
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
		Log.Debug("Instantiated {Name} with options {@Options}", nameof(AppDbContext), options);
	}

	public override void Dispose()
	{
		Log.Debug("Disposing {Name}", nameof(AppDbContext));
		base.Dispose();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		using var operation = Operation.Begin("Saving changes");
		var result = base.SaveChanges(acceptAllChangesOnSuccess);
		operation.Complete();
		return result;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured)
			return;
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
		modelBuilder.ApplyConfiguration(new ScreenshotsLibraryConfiguration());
		modelBuilder.ApplyConfiguration(new ScreenshotConfiguration());
		modelBuilder.ApplyConfiguration(new AssetConfiguration());
		modelBuilder.ApplyConfiguration(new ItemClassConfiguration());
		modelBuilder.ApplyConfiguration(new GameConfiguration());
		modelBuilder.ApplyConfiguration(new DetectorItemConfiguration());
		modelBuilder.ApplyConfiguration(new ImageConfiguration());
		modelBuilder.ApplyConfiguration(new DataSetConfiguration());
		modelBuilder.ApplyConfiguration(new WeightsLibraryConfiguration());
		modelBuilder.ApplyConfiguration(new ProfileConfiguration());
		modelBuilder.ApplyConfiguration(new ProfileItemClassConfiguration());
		modelBuilder.ApplyConfiguration(new WeightsConfiguration());
		modelBuilder.ApplyConfiguration(new AssetsLibraryConfiguration());
		modelBuilder.ApplyConfiguration(new PreemptionSettingsConfiguration());
		modelBuilder.ApplyConfiguration(new WeightsDataConfiguration());
	}
}