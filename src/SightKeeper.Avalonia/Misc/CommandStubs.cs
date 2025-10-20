using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Misc;

internal static class CommandStubs
{
	public static ICommand Enabled => new RelayCommand(() => { });
	public static ICommand Disabled => new RelayCommand(() => { }, () => false);
}