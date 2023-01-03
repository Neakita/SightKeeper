using System.Windows.Controls;
using System.Windows.Input;

namespace SightKeeper.UI.WPF.Views.Controls;

public partial class CustomHamburgerMenu
{
	public CustomHamburgerMenu() => InitializeComponent();

	private bool _isMousePressed;
	private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _isMousePressed = true;

	private void ListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _isMousePressed = false;

	private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
	{
		if (_isMousePressed) ((ListBox) sender).ReleaseMouseCapture();
	}
}