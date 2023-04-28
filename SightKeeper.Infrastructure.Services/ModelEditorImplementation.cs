using SightKeeper.Application;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Infrastructure.Services;

public sealed class ModelEditorImplementation : ModelEditor
{
	public ModelEditorImplementation(Model model, AppDbContext dbContext, bool isItemClassesInUse)
	{
		_model = model;
		_dbContext = dbContext;
		_isItemClassesInUse = isItemClassesInUse;
	}

	public void SaveChanges() => _dbContext.SaveChanges();
	public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
		await _dbContext.SaveChangesAsync(cancellationToken);
	public void RollbackChanges()
	{
		_dbContext.RollBack();
		for (int i = 0; i < _model.ItemClasses.Count; i++)
			_model.ItemClasses.RemoveAt(0);
		if (_isItemClassesInUse)
		{
			_dbContext.Entry(_model).Collection(model => model.ItemClasses).Load();
		}
	}

	public async Task RollbackChangesAsync(CancellationToken cancellationToken = default)
	{
		await _dbContext.RollBackAsync();
		for (int i = 0; i < _model.ItemClasses.Count; i++)
			_model.ItemClasses.RemoveAt(0);
		if (_isItemClassesInUse)
		{
			await _dbContext.Entry(_model).Collection(model => model.ItemClasses).LoadAsync(cancellationToken);
		}
	}

	public void Dispose()
	{
		try
		{
			RollbackChanges();
		}
		finally
		{
			_dbContext.Dispose();
		}
	}

	public async ValueTask DisposeAsync()
	{
		try
		{
			await RollbackChangesAsync();
		}
		finally
		{
			await _dbContext.DisposeAsync();
		}
	}

	private readonly Model _model;
	private readonly AppDbContext _dbContext;
	private readonly bool _isItemClassesInUse;
}
