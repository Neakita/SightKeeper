using Microsoft.EntityFrameworkCore;
using SightKeeper.Abstractions;
using SightKeeper.Abstractions.Domain;
using SightKeeper.DAL;

namespace SightKeeper.Backend.Models;

public sealed class DetectorModelsProvider : IModelsProvider<IDetectorModel>
{
	public IEnumerable<IDetectorModel> Models
	{
		get
		{
			using IAppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.DetectorModels.AsNoTracking().ToList();
		}
	}


	public DetectorModelsProvider(IAppDbProvider dbProvider)
	{
		_dbProvider = dbProvider;
	}


	private readonly IAppDbProvider _dbProvider;
}