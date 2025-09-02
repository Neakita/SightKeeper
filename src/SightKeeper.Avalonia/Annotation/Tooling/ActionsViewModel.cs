using System.Windows.Input;
using SightKeeper.Avalonia.Annotation.Tooling.Commands;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed class ActionsViewModel(
	DeleteSelectedImageCommand deleteSelectedImageCommand,
	DeleteSelectedAssetCommand deleteSelectedAssetCommand)
	: ActionsDataContext
{
	public ICommand DeleteSelectedImageCommand { get; } = deleteSelectedImageCommand;
	public ICommand DeleteSelectedAssetCommand { get; } = deleteSelectedAssetCommand;
}