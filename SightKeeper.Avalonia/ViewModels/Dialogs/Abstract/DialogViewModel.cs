using System;
using System.Reactive.Subjects;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;

public abstract class DialogViewModel : ViewModel, IDialogViewModel
{
    public IObservable<object?> Result => _result;
    protected void Return()
    {
        _result.OnNext(null);
        _result.OnCompleted();
        _result.Dispose();
    }
    private readonly Subject<object?> _result = new();
}

public abstract class DialogViewModel<TResult> : ViewModel, IDialogViewModel<TResult>
{
    public IObservable<TResult> Result => _result;
    protected void Return(TResult result)
    {
        _result.OnNext(result);
        _result.OnCompleted();
        _result.Dispose();
    }
    private readonly Subject<TResult> _result = new();
}