using System.Threading.Tasks;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabVM : ViewModel
{
	public Repository<ModelVM> ModelVMsRepository { get; }
	private readonly ModelEditor _modelEditor;

	public ModelsTabVM(Repository<ModelVM> modelVMsRepository, ModelEditor modelEditor)
	{
		ModelVMsRepository = modelVMsRepository;
		_modelEditor = modelEditor;
	}

	private async Task CreateNewModel()
	{
		ModelVM model = Locator.Resolve<ModelVM, Model>(new DetectorModel("Unnamed detector model"));
		ModelEditorDialog editorDialog = new(model);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			ModelVMsRepository.Add(model);
	}

	private async Task EditModel(ModelVM<Model> model)
	{
		ModelEditorDialog editorDialog = new(model);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
			await _modelEditor.SaveChangesAsync(model.Item);
		else
		{
			await _modelEditor.RollbackChangesAsync(model.Item);
			model.UpdateProperties();
		}
	}

	private bool CanEditModel(object parameter) => parameter is ModelVM<Model>;

	private async Task DeleteModel(ModelVM model)
	{
		MessageBoxDialog.DialogResult result = await this.ShowMessageBoxDialog($"Do you really want to remove the {model.Name}? This action cannot be undone",
			MessageBoxDialog.DialogResult.Yes | MessageBoxDialog.DialogResult.No,
			"Confirm model deletion", MaterialIconKind.TrashCanOutline);
		if (result == MessageBoxDialog.DialogResult.Yes)
			ModelVMsRepository.Remove(model);
	}

	private bool CanDeleteModel(object parameter) => parameter is ModelVM<Model>;
}