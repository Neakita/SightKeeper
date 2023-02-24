using Microsoft.EntityFrameworkCore;
using SightKeeper.Persistance;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Backend.Models;

public sealed class DetectorModelsProvider : IModelsProvider<DetectorModel>
{
	private readonly IAppDbProvider _dbProvider;


	public DetectorModelsProvider(IAppDbProvider dbProvider) => _dbProvider = dbProvider;

	public IEnumerable<DetectorModel> Models
	{
		get
		{
			using AppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.DetectorModels.AsNoTracking().ToList();
		}
	}
}