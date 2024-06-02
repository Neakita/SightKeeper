using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Data.Configuration;
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
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured)
			return;
		SetupSqlite(optionsBuilder);
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