using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Database;

public class AppDbContext : DbContext
{
	public DbSet<DataSet> DataSets { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
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
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}