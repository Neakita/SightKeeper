using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Drawing;

public interface DrawerDataContext
{
	Screenshot? Screenshot { get; }
	IReadOnlyCollection<DrawerItemDataContext> Items { get; }
	BoundedItemDataContext? SelectedItem { get; set; }
	ICommand CreateItemCommand { get; }
	bool IsEnabled { get; }
}