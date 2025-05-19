using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.DataSets.Commands;

internal sealed class CreateDataSetCommandFactory
{
	public required DialogManager DialogManager { get; init; }
	public required DataSetCreator DataSetCreator { get; init; }

	public ICommand CreateCommand()
	{
		return new AsyncRelayCommand(CreateDataSetAsync);
	}

	private async Task CreateDataSetAsync()
	{
		using CreateDataSetViewModel dialog = new();
		if (await DialogManager.ShowDialogAsync(dialog))
			DataSetCreator.Create(dialog);
	}
}