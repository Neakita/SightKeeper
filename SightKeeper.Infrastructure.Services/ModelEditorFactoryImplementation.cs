using Microsoft.EntityFrameworkCore.ChangeTracking;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

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
		bool isItemClassesLoaded = itemClassesCollectionEntry.IsLoaded;
		if (!isItemClassesLoaded) itemClassesCollectionEntry.Load();
		return new ModelEditorImplementation(model, dbContext, isItemClassesLoaded);
	}

	public async Task<ModelEditor> BeginEditAsync(Model model, CancellationToken cancellationToken = default)
	{
		AppDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
		dbContext.Models.Attach(model);
		CollectionEntry<Model, ItemClass> itemClassesCollectionEntry =
			dbContext.Entry(model).Collection(m => m.ItemClasses);
		bool isItemClassesLoaded = itemClassesCollectionEntry.IsLoaded;
		await itemClassesCollectionEntry.LoadAsync(cancellationToken);
		return new ModelEditorImplementation(model, dbContext, isItemClassesLoaded);
	}
	
	private readonly AppDbContextFactory _dbContextFactory;
}
