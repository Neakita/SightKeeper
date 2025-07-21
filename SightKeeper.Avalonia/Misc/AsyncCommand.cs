using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.Misc;

public abstract class AsyncCommand : IAsyncRelayCommand
{
	public event EventHandler? CanExecuteChanged
	{
		add => _command.CanExecuteChanged += value;
		remove => _command.CanExecuteChanged -= value;
	}

	public event PropertyChangedEventHandler? PropertyChanged
	{
		add => _command.PropertyChanged += value;
		remove => _command.PropertyChanged -= value;
	}

	public Task? ExecutionTask => _command.ExecutionTask;

	public bool CanBeCanceled => _command.CanBeCanceled;

	public bool IsCancellationRequested => _command.IsCancellationRequested;

	public bool IsRunning => _command.IsRunning;

	protected AsyncCommand()
	{
		_command = new AsyncRelayCommand(Execute, CanExecute);
	}

	public bool CanExecute(object? parameter)
	{
		return _command.CanExecute(parameter);
	}

	public void Execute(object? parameter)
	{
		_command.Execute(parameter);
	}

	public void NotifyCanExecuteChanged()
	{
		_command.NotifyCanExecuteChanged();
	}

	public Task ExecuteAsync(object? parameter)
	{
		return _command.ExecuteAsync(parameter);
	}

	public void Cancel()
	{
		_command.Cancel();
	}

	protected virtual bool CanExecute()
	{
		return true;
	}

	protected abstract Task Execute();

	private readonly AsyncRelayCommand _command;
}