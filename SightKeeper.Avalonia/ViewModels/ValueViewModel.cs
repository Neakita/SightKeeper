using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ViewModels;

internal abstract class ValueViewModel<T> : ViewModel, IDisposable
{
    public IObservable<T> ObservableValue => _valueSubject.AsObservable();

    public T Value
    {
        get => _valueSubject.Value;
        set => SetProperty(Value, value, newValue =>
        {
            OnValueChanged(newValue);
            _valueSubject.OnNext(newValue);
            OnValueNotified(newValue);
            NotifyCommandsCanExecuteChanged();
        });
    }

    public void NotifyCanExecuteChanged(IRelayCommand command) =>
        _commandsToNotify.Add(new WeakReference<IRelayCommand>(command));

    public void NotifyCanExecuteChanged(params IRelayCommand[] commands) =>
        _commandsToNotify.AddRange(commands.Select(command => new WeakReference<IRelayCommand>(command)));

    public virtual void Dispose()
    {
        _valueSubject.Dispose();
    }

    protected ValueViewModel(T value)
    {
        _valueSubject = new BehaviorSubject<T>(value);
    }

    protected virtual void OnValueChanged(T newValue)
    {
    }

    protected virtual void OnValueNotified(T newValue)
    {
    }

    private readonly BehaviorSubject<T> _valueSubject;
    private readonly List<WeakReference<IRelayCommand>> _commandsToNotify = new();

    private void NotifyCommandsCanExecuteChanged()
    {
        foreach (var commandReference in _commandsToNotify)
        {
            if (commandReference.TryGetTarget(out var command))
                command.NotifyCanExecuteChanged();
            else
                _commandsToNotify.Remove(commandReference);
        }
    }
}