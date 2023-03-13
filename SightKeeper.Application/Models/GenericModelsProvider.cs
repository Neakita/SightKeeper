using Microsoft.EntityFrameworkCore;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Models;

public sealed class GenericModelsProvider<TModel> : IModelsProvider<TModel> where TModel : Model
{
	public GenericModelsProvider(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public IEnumerable<TModel> Models
	{
		get
		{
			using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
			return dbContext.Set<TModel>().AsNoTracking().ToList();
		}
	}


	private readonly AppDbContextFactory _dbContextFactory;
}
