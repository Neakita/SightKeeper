using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DynamicData;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Data;
using SightKeeper.UI.Avalonia.Extensions;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using SightKeeper.UI.Avalonia.Views.Windows;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ModelsTabViewModel : ViewModel
{
	private readonly Repository<Model> _modelsRepository;
	private readonly ModelEditor _modelEditor;
	public ReadOnlyObservableCollection<ModelViewModel> Models => _models;

	public ModelsTabViewModel(Repository<Model> modelsRepository, ModelEditor modelEditor)
	{
		_modelsRepository = modelsRepository;
		_modelEditor = modelEditor;
		_modelsSource.AddOrUpdate(modelsRepository.Items);
		
		_modelsSource.Connect()
			.Transform(ModelViewModel.Create)
			.Bind(out _models)
			.Subscribe();
	}

	private readonly SourceCache<Model, int> _modelsSource = new(model => model.Id);
	private readonly ReadOnlyObservableCollection<ModelViewModel> _models;

	private async Task CreateNewModel()
	{
		ModelViewModel model = ModelViewModel.Create(new DetectorModel("Unnamed detector model"));
		ModelEditorDialog editorDialog = new(model);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
		{
			_modelsRepository.Add(model.Model);
			_modelsSource.AddOrUpdate(model.Model);
		}
	}

	private async Task EditModel(ModelViewModel model)
	{
		ModelEditorDialog editorDialog = new(model);
		ModelEditorDialog.DialogResult result = await this.ShowDialog(editorDialog);
		if (result == ModelEditorDialog.DialogResult.Apply)
		{
			await _modelEditor.SaveChangesAsync(model.Model);
		}
		else
		{
			await _modelEditor.RollbackChangesAsync(model.Model);
			model.UpdateProperties();
		}
	}

	private bool CanEditModel(object parameter) => parameter is ModelViewModel;

	private async Task DeleteModel(ModelViewModel model)
	{
		MessageBoxDialog.DialogResult result = await this.ShowMessageBoxDialog($"Do you really want to remove the {model.Name}? This action cannot be undone",
			MessageBoxDialog.DialogResult.Yes | MessageBoxDialog.DialogResult.No,
			"Confirm model deletion", MaterialIconKind.TrashCanOutline);
		if (result == MessageBoxDialog.DialogResult.Yes)
		{
			_modelsRepository.Remove(model.Model);
			_modelsSource.Remove(model.Model);
		}
	}

	private bool CanDeleteModel(object parameter) => parameter is ModelViewModel;
}