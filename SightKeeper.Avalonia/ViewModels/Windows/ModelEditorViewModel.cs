namespace SightKeeper.Avalonia.ViewModels.Windows;

public sealed class ModelEditorViewModel : ViewModel
{
	/*public Repository<Game> GamesRepository { get; }
	public Repository<ModelConfig> ConfigsRepository { get; }
	
	public Model Model { get; }
	public bool CanCancel { get; }

	[Reactive] public string NewItemClassName { get; set; } = string.Empty;

	[ObservableAsProperty] public bool CanSave { get; } = false;
	
	[Reactive] public int? SelectedItemIndex { get; set; }

	public ModelEditorViewModel(Model model, bool canCancel = false)
	{
		Model = model;
		CanCancel = canCancel;
		GamesRepository = Locator.Resolve<Repository<Game>>();
		ConfigsRepository = Locator.Resolve<Repository<ModelConfig>>();
		
		_disposable = this.WhenAnyValue(vm => vm.Model.Resolution.HasErrors)
			.Select(hasErrors => !hasErrors)
			.ToPropertyEx(this, vm => vm.CanSave);
	}

	public void AddNewItemClass()
	{
		ItemClass newItemClass = new(NewItemClassName);
		NewItemClassName = string.Empty;
		Model.ItemClasses.Add(newItemClass);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}
	
	private readonly IDisposable _disposable;

	[DependsOn(nameof(NewItemClassName))]
	public bool CanAddNewItemClass(object parameter) =>
		!string.IsNullOrWhiteSpace(NewItemClassName);

	public void DeleteItemClass()
	{
		if (SelectedItemIndex == null) throw new Exception();
		Model.ItemClasses.RemoveAt(SelectedItemIndex.Value);
	}

	[DependsOn(nameof(SelectedItemIndex))]
	public bool CanDeleteItemClass(object parameter) => SelectedItemIndex != null;*/
}