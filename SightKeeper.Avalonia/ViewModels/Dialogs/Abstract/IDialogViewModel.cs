using System;
using System.Reactive.Linq;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;

public interface IDialogViewModel
{
    IObservable<object?> Result { get; }
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
    IObservable<object?> IDialogViewModel.Result => Result.Select(result => (object?)result);
    new IObservable<TResult> Result { get; }
}