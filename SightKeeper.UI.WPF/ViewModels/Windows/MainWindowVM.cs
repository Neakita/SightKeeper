using System.Collections.Generic;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using ReactiveUI;

namespace SightKeeper.UI.WPF.ViewModels.Windows;

public sealed class MainWindowVM : ReactiveObject
{
	public string Str { get; } = "Hello!";


	public MainWindowVM()
	{
		
	}
}
