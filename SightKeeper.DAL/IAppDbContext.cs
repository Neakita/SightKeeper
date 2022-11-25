using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL.Members.Abstract;
using SightKeeper.DAL.Members.Common;
using SightKeeper.DAL.Members.Detector;

namespace SightKeeper.DAL;

public interface IAppDbContext
{
	public DbSet<Model> Models { get; }
	public DbSet<DetectorModel> DetectorModels { get; }
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; }
	public DbSet<DetectorItem> DetectorItems { get; }
	public DbSet<ItemClass> ItemClasses { get; }
	public DbSet<Game> Games { get; }


	int SaveChanges();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
