using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.Selection;
using Avalonia.Metadata;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

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

	public SelectionModel<DetectorAsset> ScreenshotsSelection { get; } = new();
	[Reactive] public DetectorAsset? SelectedScreenshot { get; private set; }

	[Reactive] public int? SelectedItemIndex { get; set; }

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
	
	[Reactive] public bool ItemSelectionMode { get; set; }

	public AnnotatingTabVM(Repository<DetectorModel> modelsRepository, DetectorAnnotator annotator, ModelScreenshoter screenshoter)
	{
		ModelsRepository = modelsRepository;
		Annotator = annotator;
		Screenshoter = screenshoter;
		
		ScreenshotsSelection.SelectionChanged += OnScreenshotsSelectionChanged;
		ScreenshotsSelection.SingleSelect = false;
	}

	public void DeleteSelectedScreenshots() =>
		Annotator.DeleteScreenshots(ScreenshotsSelection.SelectedIndexes);

	[DependsOn(nameof(SelectedScreenshot))]
	public bool CanDeleteSelectedScreenshots(object parameter) =>
		SelectedScreenshot != null;

	public async Task DeleteSelectedItemAsync()
	{
		SelectedItemIndex.ThrowIfNull(nameof(SelectedItemIndex));
		await Annotator.DeleteItemAsync(SelectedItemIndex!.Value);
	}

	[DependsOn(nameof(SelectedItemIndex))]
	public bool CanDeleteSelectedItem(object parameter) => SelectedItemIndex != null;

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

	private void OnScreenshotsSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<DetectorAsset> e)
	{
		Annotator.SelectedScreenshot = ScreenshotsSelection.SelectedItem;
		SelectedScreenshot = ScreenshotsSelection.SelectedItem;
	}
}