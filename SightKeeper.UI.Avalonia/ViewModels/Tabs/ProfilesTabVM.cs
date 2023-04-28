using SightKeeper.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Tabs;

public sealed class ProfilesTabVM
{
	public static ProfilesTabVM New => Locator.Resolve<ProfilesTabVM>();
}
