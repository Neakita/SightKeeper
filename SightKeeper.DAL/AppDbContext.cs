using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.DAL;

public class AppDbContext : DbContext, IAppDbContext
{
	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;

	public AppDbContext(string dataSource = "App.db") => _dataSource = dataSource;
	
	
	public void RollBack()
	{
		IEnumerable<EntityEntry> changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
		foreach (EntityEntry entry in changedEntries)
		{
			switch (entry.State)
			{
				case EntityState.Modified:
					entry.CurrentValues.SetValues(entry.OriginalValues);
					entry.State = EntityState.Unchanged;
					break;
				case EntityState.Added:
					entry.State = EntityState.Detached;
					break;
				case EntityState.Deleted:
					entry.State = EntityState.Unchanged;
					break;
			}
		}
	}

	public void RollBack<TEntity>(TEntity entity) where TEntity : class
	{
		EntityEntry<TEntity> entry = Entry(entity);
		switch (entry.State)
		{
			case EntityState.Modified:
				entry.CurrentValues.SetValues(entry.OriginalValues);
				entry.State = EntityState.Unchanged;
				break;
			case EntityState.Added:
				entry.State = EntityState.Detached;
				break;
			case EntityState.Deleted:
				entry.State = EntityState.Unchanged;
				break;
		}
	}


	private readonly string _dataSource;
	
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = _dataSource
		};
		optionsBuilder
			.UseLazyLoadingProxies()
			.UseSqlite(connectionStringBuilder.ConnectionString);
	}
}