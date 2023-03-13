using SightKeeper.Infrastructure.Data;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Models;

public sealed class GenericModelsService<TModel> : IModelsService<TModel> where TModel : Model
{
	public GenericModelsService(IModelsFactory<TModel> modelsFactory, AppDbContextFactory dbContextFactory)
	{
		_modelsFactory = modelsFactory;
		_dbContextFactory = dbContextFactory;
	}


	public TModel Create(string name, Resolution resolution)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		TModel newModel = _modelsFactory.Create(name, resolution);
		dbContext.Add(newModel);
		dbContext.SaveChanges();
		return newModel;
	}

	public void Add(TModel model)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Add(model);
		dbContext.SaveChanges();
	}

	public void Delete(TModel model)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Remove(model);
		dbContext.SaveChanges();
	}

	public void Delete(IEnumerable<TModel> models)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Set<TModel>().RemoveRange(models);
		dbContext.SaveChanges();
	}


	private readonly IModelsFactory<TModel> _modelsFactory;
	private readonly AppDbContextFactory _dbContextFactory;
}
