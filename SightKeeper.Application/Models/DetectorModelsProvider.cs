using Microsoft.EntityFrameworkCore;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Models;

public sealed class DetectorModelsProvider : IModelsProvider<DetectorModel>
{
	private readonly AppDbContextFactory _dbContextFactory;


	public DetectorModelsProvider(AppDbContextFactory dbContextFactory) => _dbContextFactory = dbContextFactory;

	public IEnumerable<DetectorModel> Models
	{
		get
		{
			using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
			return dbContext.DetectorModels.AsNoTracking().ToList();
		}
	}
}