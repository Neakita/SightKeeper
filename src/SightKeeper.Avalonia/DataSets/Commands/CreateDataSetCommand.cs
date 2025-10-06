using System;
using System.Threading.Tasks;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Misc;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class CreateDataSetCommand(
	Func<CreateDataSetViewModel> viewModelFactory,
	DialogManager dialogManager,
	DataSetCreator dataSetCreator)
	: AsyncCommand
{
	protected override async Task ExecuteAsync()
	{
		using var dialog = viewModelFactory();
		if (await dialogManager.ShowDialogAsync(dialog))
			dataSetCreator.Create(dialog);
	}
}