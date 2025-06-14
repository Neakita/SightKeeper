using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using FluentValidation;

namespace SightKeeper.Avalonia;

internal sealed class ViewModelValidator<TValidatable> : INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	public bool HasErrors => _errors.Count > 0;

	public ViewModelValidator(IValidator<TValidatable> validator, INotifyPropertyChanged propertyNotificationsSource, TValidatable validatable)
	{
		_validator = validator;
		_propertyNotificationsSource = propertyNotificationsSource;
		_validatable = validatable;
		_propertyNotificationsSource.PropertyChanged += OnViewModelPropertyChanged;
		ValidateEntireViewModel();
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
			return _errors.SelectMany(errors => errors.Value);
		string[]? errors;
		if (_errors.TryGetValue(propertyName, out errors))
			return errors;
		return Enumerable.Empty<string>();
	}

	public void Dispose()
	{
		_propertyNotificationsSource.PropertyChanged -= OnViewModelPropertyChanged;
	}

	public IDisposable SuppressValidation()
	{
		_validateOnPropertyChanged = false;
		return Disposable.Create(this, validator =>
		{
			validator._validateOnPropertyChanged = true;
			ValidateEntireViewModel();
		});
	}

	public void ValidateEntireViewModel()
	{
		var validationResult = _validator.Validate(_validatable);
		_errors = validationResult.ToDictionary();
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));
	}

	public void ValidateProperty(string propertyName)
	{
		var validationResult = _validator.Validate(_validatable, strategy => strategy.IncludeProperties(propertyName));
		if (validationResult.IsValid)
		{
			if (_errors.Remove(propertyName))
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			return;
		}
		var wasValid = !_errors.ContainsKey(propertyName);
		_errors[propertyName] = validationResult.ToDictionary().Single().Value;
		if (wasValid)
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	}

	private readonly IValidator<TValidatable> _validator;
	private readonly INotifyPropertyChanged _propertyNotificationsSource;
	private readonly TValidatable _validatable;
	private System.Collections.Generic.IDictionary<string, string[]> _errors = new Dictionary<string, string[]>();
	private bool _validateOnPropertyChanged = true;

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!_validateOnPropertyChanged)
			return;
		if (string.IsNullOrEmpty(e.PropertyName))
			ValidateEntireViewModel();
		else
			ValidateProperty(e.PropertyName);
	}
}