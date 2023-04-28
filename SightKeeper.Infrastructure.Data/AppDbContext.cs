using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Serilog;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using Splat.ModeDetection;

namespace SightKeeper.Infrastructure.Data;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
	}

	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<ItemClassGroup> ItemClassesGroups { get; set; } = null!;
	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
	public DbSet<ModelConfig> ModelConfigs { get; set; } = null!;
	public DbSet<Image> Images { get; set; } = null!;

	public void RollBack()
	{
		IEnumerable<EntityEntry> changedEntries = ChangeTracker.Entries().Where(entry => entry.State != EntityState.Unchanged);
		foreach (EntityEntry entry in changedEntries) RollbackEntry(entry);
	}

	public async Task RollBackAsync()
	{
		IEnumerable<EntityEntry> changedEntries = ChangeTracker.Entries().Where(entry => entry.State != EntityState.Unchanged);
		foreach (EntityEntry entry in changedEntries) await RollbackEntryAsync(entry);
	}

	public void RollBack<TEntity>(TEntity entity) where TEntity : class => RollbackEntry(Entry(entity));

	private void RollbackEntry(EntityEntry entry)
	{
		switch (entry.State)
		{
			case EntityState.Modified:
			case EntityState.Deleted:
				entry.State = EntityState.Modified; //Revert changes made to deleted entity.
				entry.State = EntityState.Unchanged;
				break;
			case EntityState.Added:
				entry.State = EntityState.Detached;
				break;
			case EntityState.Detached:
				entry.Reload();
				break;
		}
	}

	private async Task RollbackEntryAsync(EntityEntry entry)
	{
		switch (entry.State)
		{
			case EntityState.Modified:
			case EntityState.Deleted:
				entry.State = EntityState.Modified; //Revert changes made to deleted entity.
				entry.State = EntityState.Unchanged;
				break;
			case EntityState.Added:
				entry.State = EntityState.Detached;
				break;
			case EntityState.Detached:
				await entry.ReloadAsync();
				break;
		}
	}


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured) return;
		optionsBuilder.LogTo(Log.Verbose, LogLevel.Trace);
		optionsBuilder.EnableSensitiveDataLogging();
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = "App.db"
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
		modelBuilder.Entity<Model>().OwnsOne(model => model.Resolution).Ignore(resolution => resolution.HasErrors);
		modelBuilder.Entity<DetectorScreenshot>().HasMany<DetectorItem>().WithOne(nameof(DetectorItem.Screenshot)).OnDelete(DeleteBehavior.Cascade);
	}
}