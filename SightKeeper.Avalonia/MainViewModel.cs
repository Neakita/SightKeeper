using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia;

public sealed partial class MainViewModel : ViewModel, DialogHost
{
	public DialogManager DialogManager { get; }
	public IReadOnlyCollection<TabItemViewModel> Tabs { get; }
	[ObservableProperty] public partial TabItemViewModel SelectedTab { get; set; }

	public MainViewModel(
		DialogManager dialogManager,
		IReadOnlyCollection<TabItemViewModel> tabs)
	{
		DialogManager = dialogManager;
		Tabs = tabs;
		SelectedTab = Tabs.First();
	}
}