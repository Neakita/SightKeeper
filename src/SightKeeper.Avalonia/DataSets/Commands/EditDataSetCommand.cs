using System.Threading.Tasks;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class EditDataSetCommand(DialogManager dialogManager, DataSetEditor dataSetEditor) : AsyncCommand<DataSet<Tag, Asset>>
{
	protected override async Task ExecuteAsync(DataSet<Tag, Asset> dataSet)
	{
		EditDataSetViewModel dialog = new(dataSet);
		if (await dialogManager.ShowDialogAsync(dialog))
			dataSetEditor.Edit(dialog);
	}
}