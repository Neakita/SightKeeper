using Avalonia;
using DynamicData.Kernel;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
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
		if (SelectedScreenshot == null) throw new NullReferenceException("No selected screenshot");
		SelectedScreenshot.Items.Remove(item);
	}

	public void MarkAsAssets(IReadOnlyCollection<DetectorScreenshot> screenshots)
	{
		throw new NotImplementedException();
	}

	public void Move(DetectorItem item, Point position, Size size)
	{
		throw new NotImplementedException();
	}

	public void RemoveScreenshots(IReadOnlyCollection<DetectorScreenshot> screenshots)
	{
		if (Model == null) throw new Exception();
		// match all indexes and and remove in reverse as it is more efficient
		List<ItemWithIndex<DetectorScreenshot?>> toRemove = Model.DetectorScreenshots
			.IndexOfMany(screenshots)
			.OrderByDescending(x => x.Index)
			.ToList();
		// Fast remove because we know the index of all and we remove in order
		toRemove.ForEach(t => Model.DetectorScreenshots.RemoveAt(t.Index));
	}

	public void BeginDrawing(Point position) => _drawer.BeginDrawing(position);
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
		dbContext.Models.Where(m => m.Id == model.Id).Select(m => m.ItemClasses).Load();
	}
}
