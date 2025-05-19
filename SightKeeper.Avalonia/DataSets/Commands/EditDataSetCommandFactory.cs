using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class EditDataSetCommandFactory
{
	public required DialogManager DialogManager { get; init; }
	public required DataSetEditor DataSetEditor { get; init; }

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand<DataSet>(EditDataSetAsync!);
	}

	private async Task EditDataSetAsync(DataSet dataSet)
	{
		EditDataSetViewModel dialog = new(dataSet);
		if (await DialogManager.ShowDialogAsync(dialog))
			DataSetEditor.Edit(dialog);
	}
}