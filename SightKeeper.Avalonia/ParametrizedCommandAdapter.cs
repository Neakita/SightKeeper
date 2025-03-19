using System;
using System.Windows.Input;

namespace SightKeeper.Avalonia;

internal sealed class ParametrizedCommandAdapter : ICommand
{
	public event EventHandler? CanExecuteChanged;

	public ParametrizedCommandAdapter(ICommand command, object? parameter)
	{
		_command = command;
		_parameter = parameter;
	}

	public bool CanExecute(object? parameter)
	{
		return _command.CanExecute(_parameter);
	}

	public void Execute(object? parameter)
	{
		_command.Execute(_parameter);
	}

	private readonly ICommand _command;
	private readonly object? _parameter;
}