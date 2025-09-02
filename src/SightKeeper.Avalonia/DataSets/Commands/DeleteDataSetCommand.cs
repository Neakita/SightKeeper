using System.Threading.Tasks;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class DeleteDataSetCommand(
	DialogManager dialogManager,
	WriteRepository<DataSet> writeDataSetsRepository)
	: AsyncCommand<DataSet>
{
	protected override async Task ExecuteAsync(DataSet dataSet)
	{
		MessageBoxButtonDefinition deletionButton = new("Delete", MaterialIconKind.Delete);
		MessageBoxDialogViewModel dialog = new(
			"Data set deletion confirmation",
			$"Are you sure you want to permanently delete the data set '{dataSet.Name}'? You will not be able to recover it.",
			deletionButton,
			new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel));
		if (await dialogManager.ShowDialogAsync(dialog) == deletionButton)
			writeDataSetsRepository.Remove(dataSet);
	}
}