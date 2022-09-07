using System.Windows.Controls;

namespace SightKeeper.UI.WPF.ViewModels;

public sealed class SideMenuItemVM
{
	public string Name { get; }
	public UserControl Content { get; }

	public SideMenuItemVM(string name, UserControl content)
	{
		Name = name;
		Content = content;
	}
}
