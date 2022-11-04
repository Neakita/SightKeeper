using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Common;
using SightKeeper.DAL.Members.Detector;

namespace SightKeeper.DAL;

public class AppDbContext : DbContext
{
	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;

	public AppDbContext(string dataSource = "App.db") => _dataSource = dataSource;


	private readonly string _dataSource;
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = _dataSource
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new ProfileConfiguration());
		modelBuilder.ApplyConfiguration(new ItemClassConfiguration());
		modelBuilder.ApplyConfiguration(new ModelConfiguration());
		modelBuilder.ApplyConfiguration(new GameConfiguration());
		modelBuilder.ApplyConfiguration(new ScreenshotConfiguration());
		modelBuilder.ApplyConfiguration(new DetectorItemConfiguration());
	}
}