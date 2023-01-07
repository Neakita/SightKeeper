using MaterialDesignThemes.Wpf;
using SightKeeper.UI.WPF.Views.Windows;

namespace SightKeeper.UI.WPF.Dialogs;

public sealed class MainWindowMDDialogHostProvider : IMDDialogHostProvider<MainWindow>
{
	public MainWindowMDDialogHostProvider(MainWindow mainWindow) => _mainWindow = mainWindow;


	public DialogHost DialogHost => _mainWindow.DialogHost;


	private readonly MainWindow _mainWindow;
}