using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.Application;
using SightKeeper.Data;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Services;

public sealed class ModelEditorFactoryImplementation : ModelEditorFactory
{
	public ModelEditorFactoryImplementation(AppDbContextFactory dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public ModelEditor BeginEdit(Model model)
	{
		AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Models.Attach(model);
		CollectionEntry<Model, ItemClass> itemClassesCollectionEntry =
			dbContext.Entry(model).Collection(m => m.ItemClasses);
		itemClassesCollectionEntry.Load();
		return new ModelEditorImplementation(dbContext);
	}

	public async Task<ModelEditor> BeginEditAsync(Model model, CancellationToken cancellationToken = default)
	{
		AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		dbContext.Models.Attach(model);
		CollectionEntry<Model, ItemClass> itemClassesCollectionEntry =
			dbContext.Entry(model).Collection(m => m.ItemClasses);
		await itemClassesCollectionEntry.LoadAsync(cancellationToken);
		return new ModelEditorImplementation(dbContext);
	}
	
	private readonly AppDbContextFactory _dbContextFactory;
}
