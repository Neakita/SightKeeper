namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class AnnotatingTabViewModel : ViewModel
{
	/*public Task<IReadOnlyCollection<Model>> Models => _modelsDataAccess.GetModels();
	public DetectorAnnotator Annotator { get; }
	public ModelScreenshoter Screenshoter { get; }

	[ObservableProperty] private ItemClass? _selectedItemClass;
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

	public AnnotatingTabViewModel(DetectorAnnotator annotator, ModelScreenshoter screenshoter)
	{
		Annotator = annotator;
		Screenshoter = screenshoter;
		
		this.WhenActivated(disposables =>
		{
			Disposable.Crea
			
			var selectionChangedObservable =
				Observable.FromEventPattern<SelectionModelSelectionChangedEventArgs<DetectorAsset>>(
					handler => ScreenshotsSelection.SelectionChanged += handler,
					handler => ScreenshotsSelection.SelectionChanged -= handler);

			disposables.DisposeWith(selectionChangedObservable);
		});
		
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

	private void OnScreenshotsSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<DetectorAsset> e)
	{
		Annotator.SelectedScreenshot = ScreenshotsSelection.SelectedItem;
		SelectedScreenshot = ScreenshotsSelection.SelectedItem;
	}

	private ModelsDataAccess _modelsDataAccess;
	public ViewModelActivator Activator { get; } = new();*/
}