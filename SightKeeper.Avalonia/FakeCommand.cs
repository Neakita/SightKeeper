using System;
using System.Windows.Input;

namespace SightKeeper.Avalonia;

internal sealed class FakeCommand : ICommand
{
	public static FakeCommand Instance { get; } = new();

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public void Execute(object? parameter)
	{
	}
}