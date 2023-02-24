using SightKeeper.Persistance;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application;

public sealed class ModelEditor : IModelEditor
{
	public Model? EditableModel { get; set; }
	
	
	public void SaveChanges()
	{
		if (EditableModel == null) throw new InvalidOperationException($"{nameof(EditableModel)} was null.");
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.Models.Update(EditableModel);
		dbContext.SaveChanges();
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		if (EditableModel == null) throw new InvalidOperationException($"{nameof(EditableModel)} was null.");
		await using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.Models.Update(EditableModel);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public void DiscardChanges()
	{
		if (EditableModel == null) throw new InvalidOperationException($"{nameof(EditableModel)} was null.");
		using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.RollBack(EditableModel);
	}

	public async Task DiscardChangesAsync(CancellationToken cancellationToken = default)
	{
		if (EditableModel == null) throw new InvalidOperationException($"{nameof(EditableModel)} was null.");
		await using AppDbContext dbContext = _dbProvider.NewContext;
		dbContext.RollBack(EditableModel);
	}


	public ModelEditor(IAppDbProvider dbProvider)
	{
		_dbProvider = dbProvider;
	}

	private readonly IAppDbProvider _dbProvider;
}
