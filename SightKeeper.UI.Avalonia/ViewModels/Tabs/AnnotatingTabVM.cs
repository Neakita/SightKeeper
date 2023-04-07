using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Metadata;
using DynamicData.Kernel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class AnnotatingTabVM : ViewModel
{
	public static AnnotatingTabVM New => Locator.Resolve<AnnotatingTabVM>();
	
	public Repository<DetectorModel> ModelsRepository { get; }
	public DetectorAnnotator Annotator { get; }
	public ModelScreenshoter Screenshoter { get; }

	public ItemClass? SelectedItemClass
	{
		get => _selectedItemClass;
		set
		{
			this.RaiseAndSetIfChanged(ref _selectedItemClass, value);
			Annotator.SelectedItemClass = value;
		}
	}

	public SelectionModel<DetectorScreenshot> ScreenshotsSelection { get; } = new();
	[Reactive] public DetectorScreenshot? SelectedScreenshot { get; private set; }

	public DetectorModel? Model
	{
		get => _model;
		set
		{
			Screenshoter.Model = value;
			Annotator.Model = value;
			this.RaiseAndSetIfChanged(ref _model, value);
		}
	}

	public AnnotatingTabVM(Repository<DetectorModel> modelsRepository, DetectorAnnotator annotator, ModelScreenshoter screenshoter)
	{
		ModelsRepository = modelsRepository;
		Annotator = annotator;
		Screenshoter = screenshoter;
		
		ScreenshotsSelection.SelectionChanged += OnSelectionChanged;
		ScreenshotsSelection.SingleSelect = false;
	}

	private void OnSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<DetectorScreenshot> e)
	{
		Annotator.SelectedScreenshot = ScreenshotsSelection.SelectedItem;
		SelectedScreenshot = ScreenshotsSelection.SelectedItem;
	}

	public void NotifyKeyDown(Key key)
	{
		Model? model = Annotator.Model;
		if (model == null) return;
		if (key is < Key.D1 or > Key.D9) return;
		int itemClassIndex = key - Key.D1;
		if (itemClassIndex < model.ItemClasses.Count)
			SelectedItemClass = model.ItemClasses[itemClassIndex];
	}
	
	private DetectorModel? _model;
	private ItemClass? _selectedItemClass;

	public void DeleteSelectedScreenshots()
	{
		if (Model == null) throw new Exception();
		// match all indexes and and remove in reverse as it is more efficient
		List<ItemWithIndex<DetectorScreenshot?>> toRemove = Model.DetectorScreenshots
			.IndexOfMany(ScreenshotsSelection.SelectedItems)
			.OrderByDescending(x => x.Index)
			.ToList();

		// Fast remove because we know the index of all and we remove in order
		toRemove.ForEach(t => Model.DetectorScreenshots.RemoveAt(t.Index));
	}

	[DependsOn(nameof(Model))]
	[DependsOn(nameof(SelectedScreenshot))]
	public bool CanDeleteSelectedScreenshots(object parameter) =>
		Model != null &&
		SelectedScreenshot != null;

	public void BeginDrawing(Point position) => Annotator.BeginDrawing(position);
	public void UpdateDrawing(Point position) => Annotator.UpdateDrawing(position);
	public void EndDrawing(Point position) => Annotator.EndDrawing(position);
}