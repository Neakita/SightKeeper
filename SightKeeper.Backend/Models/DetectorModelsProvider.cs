using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Detector;

namespace SightKeeper.Backend.Models;

public sealed class DetectorModelsProvider : IModelsProvider<DetectorModel>
{
	public IEnumerable<DetectorModel> Models
	{
		get
		{
			using AppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.DetectorModels.AsNoTracking().ToList();
		}
	}


	public DetectorModelsProvider(IAppDbProvider dbProvider) => _dbProvider = dbProvider;


	private readonly IAppDbProvider _dbProvider;
}