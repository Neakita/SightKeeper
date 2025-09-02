using System.Threading.Tasks;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class EditDataSetCommand(DialogManager dialogManager, DataSetEditor dataSetEditor) : AsyncCommand<DataSet>
{
	protected override async Task ExecuteAsync(DataSet dataSet)
	{
		EditDataSetViewModel dialog = new(dataSet);
		if (await dialogManager.ShowDialogAsync(dialog))
			dataSetEditor.Edit(dialog);
	}
}