using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Backend.Data.Members;
using SightKeeper.Backend.Data.Members.Abstract;
using SightKeeper.Backend.Data.Members.Detector;

namespace SightKeeper.Backend.Data;

public class AppDbContext : DbContext
{
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;


	public AppDbContext(string dataSource = "App.db")
	{
		_dataSource = dataSource;
	}
	
	
	private readonly string _dataSource;
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionStringBuilder = new SqliteConnectionStringBuilder
		{
			DataSource = _dataSource
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Screenshot>().Ignore(screenshot => screenshot.File);
	}
}