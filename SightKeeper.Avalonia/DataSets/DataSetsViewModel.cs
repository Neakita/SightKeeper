using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.DataSets;

internal partial class DataSetsViewModel : ViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }

	public DataSetsViewModel(
		DataSetsListViewModel dataSetsListViewModel,
		DialogManager dialogManager,
		GamesDataAccess gamesDataAccess)
	{
		_dialogManager = dialogManager;
		_gamesDataAccess = gamesDataAccess;
		DataSets = dataSetsListViewModel.DataSets;
	}

	private readonly DialogManager _dialogManager;
	private readonly GamesDataAccess _gamesDataAccess;

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	[RelayCommand]
	private async Task CreateDataSetAsync()
	{
		CreateDataSetViewModel dialog = new(new GeneralDataSetEditorViewModel(_gamesDataAccess));
		if (await _dialogManager.ShowDialogAsync(dialog))
		{
		}
	}
}