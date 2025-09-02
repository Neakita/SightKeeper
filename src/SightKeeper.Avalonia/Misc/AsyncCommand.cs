using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Misc;

public abstract class AsyncCommand : IAsyncRelayCommand
{
	public event EventHandler? CanExecuteChanged
	{
		add => _commandImplementation.CanExecuteChanged += value;
		remove => _commandImplementation.CanExecuteChanged -= value;
	}

	public event PropertyChangedEventHandler? PropertyChanged
	{
		add => _commandImplementation.PropertyChanged += value;
		remove => _commandImplementation.PropertyChanged -= value;
	}

	public Task? ExecutionTask => _commandImplementation.ExecutionTask;

	public bool CanBeCanceled => _commandImplementation.CanBeCanceled;

	public bool IsCancellationRequested => _commandImplementation.IsCancellationRequested;

	public bool IsRunning => _commandImplementation.IsRunning;

	bool ICommand.CanExecute(object? parameter)
	{
		return _commandImplementation.CanExecute(parameter);
	}

	void ICommand.Execute(object? parameter)
	{
		_commandImplementation.Execute(parameter);
	}

	void IRelayCommand.NotifyCanExecuteChanged()
	{
		_commandImplementation.NotifyCanExecuteChanged();
	}

	Task IAsyncRelayCommand.ExecuteAsync(object? parameter)
	{
		return _commandImplementation.ExecuteAsync(parameter);
	}

	void IAsyncRelayCommand.Cancel()
	{
		_commandImplementation.Cancel();
	}

	protected AsyncCommand()
	{
		_commandImplementation = new AsyncRelayCommand(ExecuteAsync, CanExecute);
	}

	protected virtual bool CanExecute()
	{
		return true;
	}

	protected abstract Task ExecuteAsync();

	private readonly AsyncRelayCommand _commandImplementation;
}