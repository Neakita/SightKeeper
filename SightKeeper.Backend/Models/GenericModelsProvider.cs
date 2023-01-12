using Microsoft.EntityFrameworkCore;
using SightKeeper.Persistance;
using SightKeeper.Domain.Abstract;

namespace SightKeeper.Backend.Models;

public sealed class GenericModelsProvider<TModel> : IModelsProvider<TModel> where TModel : Model
{
	public GenericModelsProvider(IAppDbProvider dbProvider)
	{
		_dbProvider = dbProvider;
	}

	public IEnumerable<TModel> Models
	{
		get
		{
			using AppDbContext dbContext = _dbProvider.NewContext;
			return dbContext.Set<TModel>().AsNoTracking().ToList();
		}
	}


	private readonly IAppDbProvider _dbProvider;
}
