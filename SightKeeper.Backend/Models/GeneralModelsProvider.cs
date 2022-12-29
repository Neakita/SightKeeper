using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Abstract;

namespace SightKeeper.Backend.Models;

public sealed class GeneralModelsProvider : IModelsProvider<Model>
{
	public IEnumerable<Model> Models
	{
		get
		{
			using AppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.Models.AsNoTracking().ToList();
		}
	}


	public GeneralModelsProvider(IAppDbProvider dbProvider)
	{
		_dbProvider = dbProvider;
	}


	private readonly IAppDbProvider _dbProvider;
}
