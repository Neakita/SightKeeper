using Microsoft.EntityFrameworkCore;
using SightKeeper.Abstractions;
using SightKeeper.Abstractions.Domain;
using SightKeeper.DAL;

namespace SightKeeper.Backend.Models;

public sealed class GeneralModelsProvider : IModelsProvider<IModel>
{
	public IEnumerable<IModel> Models
	{
		get
		{
			using IAppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.Models.AsNoTracking().ToList();
		}
	}


	public GeneralModelsProvider(IAppDbProvider dbProvider)
	{
		_dbProvider = dbProvider;
	}


	private readonly IAppDbProvider _dbProvider;
}
