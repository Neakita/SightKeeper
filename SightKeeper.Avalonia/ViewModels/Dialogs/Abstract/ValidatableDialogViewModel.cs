using System;
using System.Reactive.Subjects;
using FluentValidation;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;

public abstract class ValidatableDialogViewModel<TValidatable> : ValidatableViewModel<TValidatable>, IDialogViewModel
    where TValidatable : class
{
    public IObservable<object?> Result => _result;
    protected ValidatableDialogViewModel(IValidator<TValidatable> validator) : base(validator) { }
    protected void Return()
    {
        _result.OnNext(null);
        _result.OnCompleted();
        _result.Dispose(); // ISSUE is it needed?
    }
    private readonly Subject<object?> _result = new();
}

public abstract class ValidatableDialogViewModel<TValidatable, TResult> : ValidatableViewModel<TValidatable>, IDialogViewModel<TResult>
    where TValidatable : class
{
    public IObservable<TResult> Result => _result;
    protected ValidatableDialogViewModel(IValidator<TValidatable> validator) : base(validator) { }
    protected void Return(TResult result)
    {
        _result.OnNext(result);
        _result.OnCompleted();
        _result.Dispose(); // ISSUE is it needed?
    }
    private readonly Subject<TResult> _result = new();
}