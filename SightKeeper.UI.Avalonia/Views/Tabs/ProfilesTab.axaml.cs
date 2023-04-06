using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;

namespace SightKeeper.UI.Avalonia.Views.Tabs;

public partial class ProfilesTab : ReactiveUserControl<ProfilesTabVM>
{
	public ProfilesTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}