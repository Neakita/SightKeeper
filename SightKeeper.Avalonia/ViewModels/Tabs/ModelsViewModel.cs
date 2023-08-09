using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class ModelsViewModel : ViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();
	public ReadOnlyObservableCollection<ModelViewModel> Models { get; }

	public ModelsViewModel(ILifetimeScope scope, ModelsListViewModel modelsListViewModel)
	{
		Models = modelsListViewModel.Models;
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
	private async Task CreateNewModel(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(_scope);
		Guard.IsNotNull(_modelCreator);
		await using var scope = _scope.BeginLifetimeScope();
		var viewModel = scope.Resolve<Dialogs.ModelEditorViewModel>();
		await viewModel.ShowDialog(this);
		if (viewModel.DialogResult != true) return;
		await _modelCreator.CreateModel(new NewModelDataDTO(ModelType.Detector, viewModel), cancellationToken);
		OnPropertyChanged(nameof(Models));
	}

	[RelayCommand(CanExecute = nameof(CanEditModel))]
	private async Task EditModel(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedModel);
		var modelToEdit = SelectedModel.Model;
		Guard.IsNotNull(_scope);
		Guard.IsNotNull(_modelEditor);
		await using var scope = _scope.BeginLifetimeScope();
		var viewModel = scope.Resolve<Dialogs.ModelEditorViewModel>();
		viewModel.SetData(modelToEdit);
		await viewModel.ShowDialog(this);
		if (viewModel.DialogResult != true) return;
		await _modelEditor.ApplyChanges(new ModelChangesDTO(modelToEdit, viewModel), cancellationToken);
		await _scope.Resolve<AppDbContext>().SaveChangesAsync(cancellationToken);
		OnPropertyChanged(nameof(Models));
	}

	private bool CanEditModel() => SelectedModel != null;

	[RelayCommand(CanExecute = nameof(CanDeleteModel))]
	private async Task DeleteModel(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedModel);
		Guard.IsNotNull(_modelsDataAccess);
		await _modelsDataAccess.RemoveModel(SelectedModel.Model, cancellationToken);
		OnPropertyChanged(nameof(Models));
	}

	private bool CanDeleteModel() => SelectedModel != null;

	private ILifetimeScope? _scope;
	[ObservableProperty] private ModelViewModel? _selectedModel;
}