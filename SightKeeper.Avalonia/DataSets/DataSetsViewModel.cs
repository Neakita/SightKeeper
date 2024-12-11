using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets;

internal partial class DataSetsViewModel : ViewModel, IDataSetsViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }

	public DataSetsViewModel(
		DataSetViewModelsObservableRepository dataSetsObservableRepository,
		DialogManager dialogManager,
		ReadDataAccess<DataSet> readDataSetsDataAccess,
		DataSetCreator dataSetCreator,
		DataSetEditor dataSetEditor,
		WriteDataAccess<DataSet> writeDataSetsDataAccess)
	{
		_dialogManager = dialogManager;
		_readDataSetsDataAccess = readDataSetsDataAccess;
		_dataSetCreator = dataSetCreator;
		_dataSetEditor = dataSetEditor;
		_writeDataSetsDataAccess = writeDataSetsDataAccess;
		DataSets = dataSetsObservableRepository.Items;
		_newDataSetDataValidator = new NewDataSetDataValidator(new DataSetDataValidator(), readDataSetsDataAccess);
	}

	private readonly DialogManager _dialogManager;
	private readonly ReadDataAccess<DataSet> _readDataSetsDataAccess;
	private readonly DataSetCreator _dataSetCreator;
	private readonly DataSetEditor _dataSetEditor;
	private readonly WriteDataAccess<DataSet> _writeDataSetsDataAccess;
	private readonly NewDataSetDataValidator _newDataSetDataValidator;

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	[RelayCommand]
	private async Task CreateDataSetAsync()
	{
		using CreateDataSetViewModel dialog = new(new DataSetEditorViewModel(_newDataSetDataValidator));
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetCreator.Create(
				dialog.DataSetEditor,
				dialog.TagsEditor.Tags,
				dialog.TypePicker.Type);
	}

	[RelayCommand]
	private async Task EditDataSetAsync(DataSet dataSet)
	{
		using EditDataSetViewModel dialog = new(dataSet, new DataSetEditorViewModel(new ExistingDataSetDataValidator(dataSet, new DataSetDataValidator(), _readDataSetsDataAccess), dataSet));
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetEditor.Edit(dataSet, dialog.DataSetEditor, dialog.TagsEditor.Tags);
	}

	[RelayCommand]
	private async Task DeleteDataSetAsync(DataSet dataSet)
	{
		MessageBoxButtonDefinition deletionButton = new("Delete", MaterialIconKind.Delete);
		MessageBoxDialogViewModel dialog = new(
			"Data set deletion confirmation",
			$"Are you sure you want to permanently delete the data set '{dataSet.Name}'? You will not be able to recover it.",
			deletionButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await _dialogManager.ShowDialogAsync(dialog) == deletionButton)
			_writeDataSetsDataAccess.Remove(dataSet);
	}

	ICommand IDataSetsViewModel.CreateDataSetCommand => CreateDataSetCommand;
	ICommand IDataSetsViewModel.EditDataSetCommand => EditDataSetCommand;
	ICommand IDataSetsViewModel.DeleteDataSetCommand => DeleteDataSetCommand;
}