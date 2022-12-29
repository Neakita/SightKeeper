using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.DAL;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;


	public void RollBack()
	{
		IEnumerable<EntityEntry> changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
		foreach (EntityEntry entry in changedEntries)
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


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured) return;
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = "App.db"
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}
}