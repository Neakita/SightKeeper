namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabViewModel : ViewModel
{
	/*public static ModelsTabVM New => Locator.Resolve<ModelsTabVM>();
	
	public Repository<Model> ModelsRepository { get; }
	[Reactive] public Model? SelectedModel { get; set; }

	public ModelsTabVM(Repository<Model> modelsRepository, ModelEditorFactory modelEditorFactory)
	{
		_modelEditorFactory = modelEditorFactory;
		ModelsRepository = modelsRepository;
	}

	public async Task CreateNewModel()
	{
		DetectorModel newModel = new("Unnamed detector model");
		ModelEditorDialog editorDialog = new(new ModelEditorViewModel(newModel, true));
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			ModelsRepository.Add(newModel);
	}

	public async Task EditModel()
	{
		if (SelectedModel == null) throw new Exception();
		await using ModelEditor editor = await _modelEditorFactory.BeginEditAsync(SelectedModel);
		using ModelEditorViewModel modelEditorViewModel = new(SelectedModel);
		ModelEditorDialog editorDialog = new(modelEditorViewModel);
		await this.ShowDialog(editorDialog);
		await editor.SaveChangesAsync();
	}

	[DependsOn(nameof(SelectedModel))]
	public bool CanEditModel(object parameter) => SelectedModel != null;

	public async Task DeleteModel()
	{
		if (SelectedModel == null) throw new Exception();
		MessageBoxDialog.DialogResult result = await this.ShowMessageBoxDialog($"Do you really want to remove the {SelectedModel.Name}? This action cannot be undone",
			MessageBoxDialog.DialogResult.Yes | MessageBoxDialog.DialogResult.No,
			"Confirm model deletion", MaterialIconKind.TrashCanOutline);
		if (result == MessageBoxDialog.DialogResult.Yes)
			ModelsRepository.Remove(SelectedModel);
	}

	[DependsOn(nameof(SelectedModel))]
	public bool CanDeleteModel(object parameter) => SelectedModel != null;
	
	private readonly ModelEditorFactory _modelEditorFactory;*/
}