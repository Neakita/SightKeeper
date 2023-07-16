using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using FluentValidation.Results;

namespace SightKeeper.Avalonia.ViewModels;

public abstract class ValidatableViewModel<TValidatable> : ViewModel, INotifyDataErrorInfo where TValidatable : class
{
    public ValidatableViewModel(IValidator<TValidatable> validator)
    {
        _validator = validator;
        PropertyChanged += OnPropertyChanged;
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return ValidationResult.Errors.Select(error => error.ErrorMessage);
        return ValidationResult.Errors.Where(error => error.PropertyName == propertyName)
            .Select(error => error.ErrorMessage);
    }

    public bool HasErrors => !ValidationResult.IsValid;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    private readonly IValidator<TValidatable> _validator;
    
    protected ValidationResult ValidationResult = new();

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var validatable = this as TValidatable;
        Guard.IsNotNull(validatable);
        var validationResult = _validator.Validate(validatable);
        var propertiesToValidate = ValidationResult.Errors.Select(error => error.PropertyName)
            .Union(validationResult.Errors.Select(error => error.PropertyName)).ToList();
        ValidationResult = validationResult;
        foreach (var property in propertiesToValidate)
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
    }
}