using SightKeeper.Infrastructure.Data;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public sealed class ModelEditorImplementation : ModelEditor
{
	public void SaveChanges(Model model)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Models.Update(model);
		dbContext.SaveChanges();
	}

	public async Task SaveChangesAsync(Model model, CancellationToken cancellationToken = default)
	{
		await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		dbContext.Models.Update(model);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public void RollbackChanges(Model model)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.RollBack(model);
	}

	public async Task RollbackChangesAsync(Model model, CancellationToken cancellationToken = default)
	{
		await using AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		dbContext.RollBack(model);
	}
	
	public ModelEditorImplementation(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	private readonly AppDbContextFactory _dbContextFactory;
}
