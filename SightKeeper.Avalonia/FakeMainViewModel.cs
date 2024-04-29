using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia;

internal sealed class FakeMainViewModel : ViewModel, IMainViewModel
{
	public DialogManager DialogManager { get; } = new();
	public IReadOnlyCollection<TabItem> Tabs { get; }
	public TabItem SelectedTab { get; set; }

	public FakeMainViewModel()
	{
		Tabs =
		[
			CreateTab<FakeSettingsViewModel>(MaterialIconKind.Cog, "Settings")
		];
		SelectedTab = Tabs.First();
	}

	private static TabItem CreateTab<TViewModel>(
		MaterialIconKind iconKind,
		string header)
		where TViewModel : ViewModel, new()
	{
		return new TabItem(iconKind, header, new TViewModel());
	}
}