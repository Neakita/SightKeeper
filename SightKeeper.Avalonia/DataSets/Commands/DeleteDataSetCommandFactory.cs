using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class DeleteDataSetCommandFactory
{
	public DeleteDataSetCommandFactory(DialogManager dialogManager, WriteDataAccess<DataSet> writeDataSetsDataAccess)
	{
		_dialogManager = dialogManager;
		_writeDataSetsDataAccess = writeDataSetsDataAccess;
	}

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<DataSet>(DeleteDataSetAsync!);
	}

	private readonly DialogManager _dialogManager;
	private readonly WriteDataAccess<DataSet> _writeDataSetsDataAccess;

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
}