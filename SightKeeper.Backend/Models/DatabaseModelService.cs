using SightKeeper.Backend.Abstract;
using SightKeeper.Backend.Models.Abstract;
using SightKeeper.DAL;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models;

public sealed class DatabaseModelService : IModelService
{
	public IEnumerable<Model> Models { get; } = Enumerable.Empty<Model>();


	public DatabaseModelService(IAppDbFactory dbFactory, INewModelNameValidator nameValidator)
	{
		_dbFactory = dbFactory;
		_nameValidator = nameValidator;
	}
	
	public void AddModel(Model model)
	{
		if (!_nameValidator.IsValidName(model.Name, out string? message))
			throw new ArgumentException($"Invalid model name: {message}");
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.Add(model);
		dbContext.SaveChanges();
	}

	public async Task AddModelAsync(Model model, CancellationToken cancellationToken = default)
	{
		if (!_nameValidator.IsValidName(model.Name, out string? message))
			throw new ArgumentException($"Invalid model name: {message}");
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.Add(model);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public void DeleteModel(Model model)
	{
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.Remove(model);
		dbContext.SaveChanges();
	}

	public async Task DeleteModelAsync(Model model, CancellationToken cancellationToken)
	{
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.Remove(model);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public void DeleteModels(IEnumerable<Model> models)
	{
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.RemoveRange(models);
		dbContext.SaveChanges();
	}

	public async Task DeleteModelsAsync(IEnumerable<Model> models, CancellationToken cancellationToken = default)
	{
		using IAppDbContext dbContext = _dbFactory.NewContext;
		dbContext.Models.RemoveRange(models);
		await dbContext.SaveChangesAsync(cancellationToken);
	}


	private readonly IAppDbFactory _dbFactory;
	private readonly INewModelNameValidator _nameValidator;
}
