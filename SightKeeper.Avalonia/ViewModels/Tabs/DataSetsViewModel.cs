using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

internal partial class DataSetsViewModel : ViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSetsViewModels { get; }

	public DataSetsViewModel(
		ILifetimeScope scope,
		DataSetsListViewModel dataSetsListViewModel,
		DataSetsDataAccess dataSetsDataSetsDataSetsDataAccess,
		DataSetCreator dataSetCreator,
		DataSetEditor dataSetEditor)
	{
		DataSetsViewModels = dataSetsListViewModel.DataSets;
		_scope = scope;
		_dataSetsDataAccess = dataSetsDataSetsDataSetsDataAccess;
		_dataSetCreator = dataSetCreator;
		_dataSetEditor = dataSetEditor;
	}

	private readonly ILifetimeScope _scope;
	private readonly DataSetsDataAccess _dataSetsDataAccess;
	private readonly DataSetCreator _dataSetCreator;
	private readonly DataSetEditor _dataSetEditor;

	[RelayCommand]
	private async Task CreateNewDataSet(CancellationToken cancellationToken)
	{
		await using var scope = _scope.BeginLifetimeScope(this);
		var viewModel = scope.Resolve<DataSetCreatingViewModel>();
		/*var applied = await this.ShowDialogAsync(viewModel);
		if (applied)
			_dataSetCreator.CreateDataSet(new NewDataSetInfoDTO(viewModel));*/
	}

	[RelayCommand(CanExecute = nameof(CanEditDataSet))]
	private async Task EditDataSet(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		var dataSetToEdit = SelectedDataSetViewModel.DataSet;
		await using var scope = _scope.BeginLifetimeScope(this);
		var viewModel = scope.Resolve<DataSetEditingViewModel>(new PositionalParameter(0, dataSetToEdit));
		/*var applied = await this.ShowDialogAsync(viewModel);
		if (applied)
			await _dataSetEditor.ApplyChanges(new DataSetChangesDTO(dataSetToEdit, viewModel), cancellationToken);*/
	}

	private bool CanEditDataSet() => SelectedDataSetViewModel != null;

	[RelayCommand(CanExecute = nameof(CanDeleteDataSet))]
	private void DeleteDataSet()
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		Guard.IsNotNull(_dataSetsDataAccess);
		_dataSetsDataAccess.RemoveDataSet(SelectedDataSetViewModel.DataSet);
	}

	private bool CanDeleteDataSet() => SelectedDataSetViewModel != null;

	[RelayCommand(CanExecute = nameof(CanEditWeights))]
	private async Task EditWeights()
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		var dialogViewModel = _scope.Resolve<WeightsEditorViewModel>();
		dialogViewModel.SetLibrary(SelectedDataSetViewModel.DataSet.Weights);
		/*await this.ShowDialogAsync(dialogViewModel);*/
	}

	private bool CanEditWeights() => SelectedDataSetViewModel != null;

	[ObservableProperty] private DataSetViewModel? _selectedDataSetViewModel;
}