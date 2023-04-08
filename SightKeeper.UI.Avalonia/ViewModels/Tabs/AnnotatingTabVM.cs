using Avalonia;
using Avalonia.Controls.Selection;
using Avalonia.Metadata;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
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
		
		ScreenshotsSelection.SelectionChanged += OnScreenshotsSelectionChanged;
		ScreenshotsSelection.SingleSelect = false;
	}

	public void DeleteSelectedScreenshots()
	{
		Annotator.RemoveScreenshots(ScreenshotsSelection.SelectedIndexes);
	}

	[DependsOn(nameof(Model))]
	[DependsOn(nameof(SelectedScreenshot))]
	public bool CanDeleteSelectedScreenshots(object parameter) =>
		Model != null &&
		SelectedScreenshot != null;

	public bool BeginDrawing(Point position) => Annotator.BeginDrawing(position);
	public void UpdateDrawing(Point position) => Annotator.UpdateDrawing(position);
	public void EndDrawing(Point position) => Annotator.EndDrawing(position);

	public void SelectItemClassByIndex(int itemClassIndex)
	{
		Model.ThrowIfNull(nameof(Model));
		if (itemClassIndex < Model!.ItemClasses.Count)
			SelectedItemClass = Model.ItemClasses[itemClassIndex];
	}
	
	private DetectorModel? _model;
	private ItemClass? _selectedItemClass;

	private void OnScreenshotsSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<DetectorScreenshot> e)
	{
		Annotator.SelectedScreenshot = ScreenshotsSelection.SelectedItem;
		SelectedScreenshot = ScreenshotsSelection.SelectedItem;
	}
}