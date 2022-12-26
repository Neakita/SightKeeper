using Microsoft.EntityFrameworkCore;
using SightKeeper.Abstractions;
using SightKeeper.DAL.Domain.Abstract;
using SightKeeper.DAL.Domain.Common;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.DAL;

public interface IAppDbContext : IDbContext
{
	public DbSet<Model> Models { get; }
	public DbSet<DetectorModel> DetectorModels { get; }
	public DbSet<DetectorScreenshot> DetectorScreenshots { get; }
	public DbSet<DetectorItem> DetectorItems { get; }
	public DbSet<ItemClass> ItemClasses { get; }
	public DbSet<Game> Games { get; }
}
