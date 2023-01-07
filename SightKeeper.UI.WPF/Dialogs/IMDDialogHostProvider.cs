using System.Windows;
using MaterialDesignThemes.Wpf;

namespace SightKeeper.UI.WPF.Dialogs;

public interface IMDDialogHostProvider<TWindow> where TWindow : Window
{
	DialogHost DialogHost { get; }
}
