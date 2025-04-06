using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;

namespace SightKeeper.Avalonia.Annotation.Drawing.Design;

internal sealed class DesignBoundingDrawerDataContext : BoundingDrawerDataContext
{
	public ICommand CreateItemCommand => new RelayCommand(() => { });
}