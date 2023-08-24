using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using FluentValidation.Results;

namespace SightKeeper.Avalonia.ViewModels;

public abstract class ValidatableViewModel<TValidatable> : ViewModel, INotifyDataErrorInfo where TValidatable : class
{
    public IValidator<TValidatable> Validator { get; }
    
    public ValidatableViewModel(IValidator<TValidatable> validator)
    {
        Validator = validator;
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
    
    protected ValidationResult ValidationResult = new();

    protected async Task<bool> Validate()
    {
        var validatable = this as TValidatable;
        Guard.IsNotNull(validatable);
        var validationResult = await Validator.ValidateAsync(validatable) ;
        UpdateValidationResult(validationResult);
        return validationResult.IsValid;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _ = Validate();
    }

    private void UpdateValidationResult(ValidationResult validationResult)
    {
        var propertiesToValidate = ValidationResult.Errors.Select(error => error.PropertyName)
            .Union(validationResult.Errors.Select(error => error.PropertyName)).ToList();
        ValidationResult = validationResult;
        foreach (var property in propertiesToValidate)
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
    }
}