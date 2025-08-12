using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface ActionsDataContext
{
	ICommand DeleteSelectedImageCommand { get; }
	ICommand DeleteSelectedAssetCommand { get; }
}