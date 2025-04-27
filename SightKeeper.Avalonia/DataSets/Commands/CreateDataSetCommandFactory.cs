using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class CreateDataSetCommandFactory
{
	public CreateDataSetCommandFactory(
		ReadDataAccess<DataSet> readDataSetsDataAccess,
		DialogManager dialogManager,
		DataSetCreator dataSetCreator)
	{
		_dialogManager = dialogManager;
		_dataSetCreator = dataSetCreator;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand(CreateDataSetAsync);
	}

	private readonly DialogManager _dialogManager;
	private readonly DataSetCreator _dataSetCreator;

	private async Task CreateDataSetAsync()
	{
		using CreateDataSetViewModel dialog = new();
		if (await _dialogManager.ShowDialogAsync(dialog))
			_dataSetCreator.Create(
				dialog,
				dialog.TypePicker.SelectedType);
	}
}