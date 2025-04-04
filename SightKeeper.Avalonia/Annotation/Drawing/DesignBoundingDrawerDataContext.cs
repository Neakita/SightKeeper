using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DesignBoundingDrawerDataContext : BoundingDrawerDataContext
{
	public ICommand CreateItemCommand => new RelayCommand(() => { });
}