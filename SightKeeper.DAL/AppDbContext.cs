using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Common.Modifiers;
using SightKeeper.DAL.Domain.Common.Synergies;
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

	public DbSet<Profile> Profiles { get; set; } = null!;
	public DbSet<ProfileComponent> ProfileComponents { get; set; } = null!;
	public DbSet<ItemClassGroup> ItemClassesGroups { get; set; } = null!;
	public DbSet<Model> Models { get; set; } = null!;
	public DbSet<DetectorModel> DetectorModels { get; set; } = null!;
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; set; } = null!;
	public DbSet<DetectorItem> DetectorItems { get; set; } = null!;
	public DbSet<ItemClass> ItemClasses { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
	public DbSet<Modifier> Modifiers { get; set; } = null!;
	public DbSet<Synergy> Synergies { get; set; } = null!;


	public void RollBack()
	{
		IEnumerable<EntityEntry> changedEntries = ChangeTracker.Entries().Where(entry => entry.State != EntityState.Unchanged);
		foreach (EntityEntry entry in changedEntries) RollbackEntry(entry);
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


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured) return;
		SqliteConnectionStringBuilder connectionStringBuilder = new()
		{
			DataSource = "App.db"
		};
		optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<AdaptiveAreaModifier>();
		builder.Entity<AdaptiveResolutionModifier>();
		builder.Entity<PassiveVariableResolutionModifier>();
		builder.Entity<ResolutionMultiplierModifier>();
		builder.Entity<VariableResolutionModifier>();
		builder.Entity<KeyHoldSynergy>();
		builder.Entity<KeyWrapper>();
		builder.Entity<MultiKeySwitchSynergy>();
		builder.Entity<SingleKeySwitchSynergy>();
		base.OnModelCreating(builder);
	}
}