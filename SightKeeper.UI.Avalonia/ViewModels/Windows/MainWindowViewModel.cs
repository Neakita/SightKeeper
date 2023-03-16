using System.Collections.Generic;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using SightKeeper.UI.Avalonia.Views.Tabs;

namespace SightKeeper.UI.Avalonia.ViewModels.Windows;

public class MainWindowViewModel : ViewModelBase
{
	public ICollection<TabItemViewModel> Tabs { get; } = new List<TabItemViewModel>
	{
		new("Profiles", new ProfilesTab()),
		new("Models", new ModelsTab())
	};

	[Reactive] public TabItemViewModel SelectedTab { get; set; }


	public MainWindowViewModel()
	{
		SelectedTab = Tabs.First();
	}
}