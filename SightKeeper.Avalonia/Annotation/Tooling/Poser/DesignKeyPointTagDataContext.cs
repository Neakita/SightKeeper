using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Annotation.Tooling.Poser;

internal sealed class DesignKeyPointTagDataContext : KeyPointTagDataContext
{
	public string Name { get; }
	public ICommand DeleteKeyPointCommand => new RelayCommand(() => { }, () => _canExecuteCommand);

	public DesignKeyPointTagDataContext(string name, bool canExecuteCommand)
	{
		Name = name;
		_canExecuteCommand = canExecuteCommand;
	}

	private readonly bool _canExecuteCommand;
}