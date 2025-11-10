using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Annotation.Tooling.Actions;

internal sealed class DesignActionsDataContext : ActionsDataContext
{
	public ICommand DeleteSelectedImageCommand { get; } = new RelayCommand(() => { });
	public ICommand DeleteSelectedAssetCommand { get; } = new RelayCommand(() => { });
}