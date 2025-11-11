using System.Threading.Tasks;
using Material.Icons;
using SightKeeper.Application.Misc;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class DeleteDataSetCommand(
	DialogManager dialogManager,
	WriteRepository<DataSet<Tag, Asset>> writeDataSetsRepository)
	: AsyncCommand<DataSet<Tag, Asset>>
{
	protected override async Task ExecuteAsync(DataSet<Tag, Asset> dataSet)
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