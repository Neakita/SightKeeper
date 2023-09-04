using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using FluentValidation.Results;

namespace SightKeeper.Avalonia.ViewModels;

public abstract class ValidatableViewModel<TValidatable> : ViewModel, INotifyDataErrorInfo where TValidatable : class
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public IObservable<DataErrorsChangedEventArgs> ErrorsChangedObservable =>
        Observable.FromEventPattern<DataErrorsChangedEventArgs>(
                handler => ErrorsChanged += handler,
                handler => ErrorsChanged -= handler)
            .Select(data => data.EventArgs);
    public bool HasErrors => !_validationResult.IsValid;
    
    public ValidatableViewModel(IValidator<TValidatable> validator)
    {
        _validator = validator;
        PropertyChanged += OnPropertyChanged;
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return _validationResult.Errors.Select(error => error.ErrorMessage);
        return _validationResult.Errors.Where(error => error.PropertyName == propertyName)
            .Select(error => error.ErrorMessage);
    }

    protected async Task<bool> Validate()
    {
        var validatable = this as TValidatable;
        Guard.IsNotNull(validatable);
        var validationResult = await _validator.ValidateAsync(validatable) ;
        UpdateValidationResult(validationResult);
        return validationResult.IsValid;
    }
    
    private readonly IValidator<TValidatable> _validator;
    private ValidationResult _validationResult = new();

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _ = Validate();
    }

    private void UpdateValidationResult(ValidationResult validationResult)
    {
        var propertiesToValidate = _validationResult.Errors.Select(error => error.PropertyName)
            .Union(validationResult.Errors.Select(error => error.PropertyName)).ToList();
        _validationResult = validationResult;
        foreach (var property in propertiesToValidate)
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
    }
}