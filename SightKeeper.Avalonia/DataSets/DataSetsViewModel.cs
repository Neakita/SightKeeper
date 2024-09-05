using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
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
		DataSetCreator dataSetCreator)
	{
		_dialogManager = dialogManager;
		_gamesDataAccess = gamesDataAccess;
		_dataSetCreator = dataSetCreator;
		DataSets = dataSetsListViewModel.DataSets;
		_newDataSetDataValidator = new NewDataSetDataValidator(new DataSetDataValidator(), dataSetsDataAccess);
	}

	private readonly DialogManager _dialogManager;
	private readonly GamesDataAccess _gamesDataAccess;
	private readonly DataSetCreator _dataSetCreator;
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
				dialog.DataSetType);
	}
}