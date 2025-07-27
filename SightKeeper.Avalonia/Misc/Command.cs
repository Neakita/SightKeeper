using System;
using System.Windows.Input;

namespace SightKeeper.Avalonia.Misc;

public abstract class Command : ICommand
{
	public event EventHandler? CanExecuteChanged;

	bool ICommand.CanExecute(object? parameter)
	{
		return CanExecute;
	}

	void ICommand.Execute(object? parameter)
	{
		Execute();
	}

	protected virtual bool CanExecute => true;

	protected void RaiseCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(null, EventArgs.Empty);
	}

	protected abstract void Execute();
}