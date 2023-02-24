using SightKeeper.Infrastructure.Data;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Models;

public sealed class GenericModelsService<TModel> : IModelsService<TModel> where TModel : Model
{
	public GenericModelsService(IModelsFactory<TModel> modelsFactory, IAppDbProvider dbProvider)
	{
		_modelsFactory = modelsFactory;
		_dbProvider = dbProvider;
	}


	public TModel Create(string name, Resolution resolution)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		TModel newModel = _modelsFactory.Create(name, resolution);
		dbContext.Add(newModel);
		dbContext.SaveChanges();
		return newModel;
	}

	public void Add(TModel model)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.Add(model);
		dbContext.SaveChanges();
	}

	public void Delete(TModel model)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.Remove(model);
		dbContext.SaveChanges();
	}

	public void Delete(IEnumerable<TModel> models)
	{
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.Set<TModel>().RemoveRange(models);
		dbContext.SaveChanges();
	}


	private readonly IModelsFactory<TModel> _modelsFactory;
	private readonly IAppDbProvider _dbProvider;
}
