using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class ModelsViewModel : ViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public Task<IReadOnlyCollection<Model>> Models
	{
		get
		{
			if (_modelsDataAccess == null) return Task.FromResult((IReadOnlyCollection<Model>)new List<Model>());
			return _modelsDataAccess.GetModels();
		}
	}

	public Model? SelectedModel { get; set; }

	public ModelsViewModel(ILifetimeScope scope)
	{
		this.WhenActivated(disposables =>
		{
			_scope = scope.BeginLifetimeScope(this);
			_scope.DisposeWith(disposables);
			_modelsDataAccess = _scope.Resolve<ModelsDataAccess>();
			_modelCreator = _scope.Resolve<ModelCreator>();
			_modelEditor = _scope.Resolve<ModelEditor>();
			OnPropertyChanged(nameof(Models));
		});
	}

	private ModelsDataAccess? _modelsDataAccess;
	private ModelCreator? _modelCreator;
	private ModelEditor? _modelEditor;

	[RelayCommand]
	private async Task CreateNewModel()
	{
		Guard.IsNotNull(_scope);
		Guard.IsNotNull(_modelCreator);
		await using var scope = _scope.BeginLifetimeScope();
		var viewModel = scope.Resolve<ModelEditorViewModel>();
		await viewModel.ShowDialog(this);
		if (viewModel.DialogResult != true) return;
		_modelCreator.CreateModel(new NewModelDataDTO(ModelType.Detector, viewModel));
		await _scope.Resolve<AppDbContext>().SaveChangesAsync();
		OnPropertyChanged(nameof(Models));
	}

	[RelayCommand(CanExecute = nameof(CanEditModel))]
	private async Task EditModel()
	{
		var modelToEdit = SelectedModel;
		Guard.IsNotNull(_scope);
		Guard.IsNotNull(_modelEditor);
		Guard.IsNotNull(modelToEdit);
		await using var scope = _scope.BeginLifetimeScope();
		var viewModel = scope.Resolve<ModelEditorViewModel>();
		viewModel.SetData(modelToEdit);
		await viewModel.ShowDialog(this);
		if (viewModel.DialogResult != true) return;
		_modelEditor.ApplyChanges(new ModelChangesDTO(modelToEdit, viewModel));
		await _scope.Resolve<AppDbContext>().SaveChangesAsync();
		OnPropertyChanged(nameof(Models));
	}

	private bool CanEditModel() => SelectedModel != null;

	[RelayCommand(CanExecute = nameof(CanDeleteModel))]
	private async Task DeleteModel()
	{
		Guard.IsNotNull(SelectedModel);
		Guard.IsNotNull(_modelsDataAccess);
		await _modelsDataAccess.RemoveModel(SelectedModel);
		OnPropertyChanged(nameof(Models));
	}

	private bool CanDeleteModel() => SelectedModel != null;

	private ILifetimeScope? _scope;
}