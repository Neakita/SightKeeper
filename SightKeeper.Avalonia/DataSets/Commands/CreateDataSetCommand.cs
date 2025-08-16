using System.Threading.Tasks;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class CreateDataSetCommand(
	DialogManager dialogManager,
	DataSetCreator dataSetCreator)
	: AsyncCommand
{
	protected override async Task ExecuteAsync()
	{
		using CreateDataSetViewModel dialog = new();
		if (await dialogManager.ShowDialogAsync(dialog))
			dataSetCreator.Create(dialog);
	}
}