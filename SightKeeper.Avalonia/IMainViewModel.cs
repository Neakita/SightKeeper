using System.Collections.Generic;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia;

internal interface IMainViewModel : DialogHost
{
	IReadOnlyCollection<TabItem> Tabs { get; }
	TabItem SelectedTab { get; set; }
}