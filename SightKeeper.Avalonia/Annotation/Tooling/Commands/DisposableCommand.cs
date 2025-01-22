using System;
using System.Windows.Input;

namespace SightKeeper.Avalonia.Annotation.Tooling.Commands;

public sealed class DisposableCommand : ICommand, IDisposable
{
	public event EventHandler? CanExecuteChanged
	{
		add => _command.CanExecuteChanged += value;
		remove => _command.CanExecuteChanged -= value;
	}

	public DisposableCommand(ICommand command, IDisposable disposable)
	{
		_command = command;
		_disposable = disposable;
	}

	public bool CanExecute(object? parameter)
	{
		return _command.CanExecute(parameter);
	}

	public void Execute(object? parameter)
	{
		_command.Execute(parameter);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly ICommand _command;
	private readonly IDisposable _disposable;
}