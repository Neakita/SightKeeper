using System;
using System.Threading.Tasks;
using Avalonia.Metadata;
using Material.Icons;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Application;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Common;
using SightKeeper.Avalonia.Extensions;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM : ViewModel
{
	public static ModelsTabVM New => Locator.Resolve<ModelsTabVM>();
	
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
		ModelEditorDialog editorDialog = new(ModelEditorVM.Create(newModel));
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			ModelsRepository.Add(newModel);
	}

	public async Task EditModel()
	{
		if (SelectedModel == null) throw new Exception();
		await using ModelEditor editor = await _modelEditorFactory.BeginEditAsync(SelectedModel);
		using ModelEditorVM modelEditorVM = ModelEditorVM.Create(SelectedModel);
		ModelEditorDialog editorDialog = new(modelEditorVM);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			await editor.SaveChangesAsync();
		else
			await editor.RollbackChangesAsync();
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
	
	private readonly ModelEditorFactory _modelEditorFactory;
}