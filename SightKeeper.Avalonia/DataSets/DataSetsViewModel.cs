using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets;

internal partial class DataSetsViewModel : ViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }

	public DataSetsViewModel(
		DataSetsListViewModel dataSetsListViewModel,
		DialogManager dialogManager,
		GamesDataAccess gamesDataAccess,
		ReadDataAccess<DataSet> dataSetsDataAccess,
		DataSetCreator dataSetCreator,
		DataSetEditor dataSetEditor)
	{
		_dialogManager = dialogManager;
		_gamesDataAccess = gamesDataAccess;
		_dataSetsDataAccess = dataSetsDataAccess;
		_dataSetCreator = dataSetCreator;
		_dataSetEditor = dataSetEditor;
		DataSets = dataSetsListViewModel.DataSets;
		_newDataSetDataValidator = new NewDataSetDataValidator(new DataSetDataValidator(), dataSetsDataAccess);
	}

	private readonly DialogManager _dialogManager;
	private readonly GamesDataAccess _gamesDataAccess;
	private readonly ReadDataAccess<DataSet> _dataSetsDataAccess;
	private readonly DataSetCreator _dataSetCreator;
	private readonly DataSetEditor _dataSetEditor;
	private readonly NewDataSetDataValidator _newDataSetDataValidator;

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	[RelayCommand]
	private async Task CreateDataSetAsync()
	{
		using CreateDataSetViewModel dialog = new(new DataSetEditorViewModel(_gamesDataAccess, _newDataSetDataValidator));
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetCreator.Create(
				dialog.DataSetEditor,
				dialog.TagsEditor.Tags,
				dialog.TypePicker.Type);
	}

	[RelayCommand]
	private async Task EditDataSetAsync(DataSet dataSet)
	{
		using EditDataSetViewModel dialog = new(dataSet, new DataSetEditorViewModel(_gamesDataAccess, new ExistingDataSetDataValidator(dataSet, new DataSetDataValidator(), _dataSetsDataAccess), dataSet));
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetEditor.Edit(dataSet, dialog.DataSetEditor, dialog.TagsEditor.Tags);
	}
}