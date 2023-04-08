using Avalonia;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;

namespace SightKeeper.Infrastructure.Services;

public sealed class DetectorAnnotatorImplementation : ReactiveObject, DetectorAnnotator
{
	public DetectorModel? Model
	{
		get => _model;
		set
		{
			this.RaiseAndSetIfChanged(ref _model, value);
			if (_model != null) LoadItemClasses(_model);
		}
	}

	public DetectorScreenshot? SelectedScreenshot
	{
		get => _selectedScreenshot;
		set
		{
			this.RaiseAndSetIfChanged(ref _selectedScreenshot, value);
			_drawer.Screenshot = value;
		}
	}

	public ItemClass? SelectedItemClass
	{
		get => _selectedItemClass;
		set
		{
			this.RaiseAndSetIfChanged(ref _selectedItemClass, value);
			_drawer.ItemClass = value;
		}
	}

	public DetectorAnnotatorImplementation(AnnotatorDrawer drawer, AppDbContextFactory dbContextFactory)
	{
		_drawer = drawer;
		_dbContextFactory = dbContextFactory;
	}

	public void RemoveItem(DetectorItem item)
	{
		SelectedScreenshot.ThrowIfNull(nameof(SelectedScreenshot));
		SelectedScreenshot!.Items.Remove(item);
	}

	public void MarkAsAssets(IReadOnlyCollection<int> screenshotsIndexes)
	{
		Model.ThrowIfNull(nameof(Model));
		foreach (int index in screenshotsIndexes)
			Model!.DetectorScreenshots[index].IsAsset = true;
	}

	public void RemoveScreenshots(IReadOnlyCollection<int> screenshotsIndexes)
	{
		Model.ThrowIfNull(nameof(Model));
		foreach (int index in screenshotsIndexes.OrderDescending())
			Model!.DetectorScreenshots.RemoveAt(index);
	}

	public bool BeginDrawing(Point position) => _drawer.BeginDrawing(position);
	public void UpdateDrawing(Point position) => _drawer.UpdateDrawing(position);
	public void EndDrawing(Point position) => _drawer.EndDrawing(position);
	
	private readonly AnnotatorDrawer _drawer;
	private readonly AppDbContextFactory _dbContextFactory;
	private DetectorScreenshot? _selectedScreenshot;
	private ItemClass? _selectedItemClass;
	private DetectorModel? _model;

	private void LoadItemClasses(Model model)
	{
		using AppDbContext dbContext = _dbContextFactory.CreateDbContext();
		dbContext.Models.Attach(model);
		dbContext.Entry(model).Collection(m => m.ItemClasses).Load();
	}
}