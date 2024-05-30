using System;
using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using FluentValidation;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia;

internal sealed class ViewModelValidator<TValidatable> : INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	public bool HasErrors => _errors.Count > 0;

	public ViewModelValidator(IValidator<TValidatable> validator, ViewModel viewModel, TValidatable validatable)
	{
		_validator = validator;
		_viewModel = viewModel;
		_validatable = validatable;
		_viewModel.PropertyChanged += OnViewModelPropertyChanged;
		ValidateEntireViewModel();
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
			return _errors.SelectMany(errors => errors.Value);
		if (_errors.TryGetValue(propertyName, out var errors))
			return errors;
		return Enumerable.Empty<string>();
	}

	public void Dispose()
	{
		_viewModel.PropertyChanged -= OnViewModelPropertyChanged;
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

	private readonly IValidator<TValidatable> _validator;
	private readonly ViewModel _viewModel;
	private readonly TValidatable _validatable;
	private System.Collections.Generic.IDictionary<string, string[]> _errors = ImmutableDictionary<string, string[]>.Empty;
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

	private void ValidateEntireViewModel()
	{
		var validationResult = _validator.Validate(_validatable);
		_errors = validationResult.ToDictionary();
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));
	}

	private void ValidateProperty(string propertyName)
	{
		var validationResult = _validator.Validate(_validatable, strategy => strategy.IncludeProperties(propertyName));
		if (validationResult.IsValid)
			_errors.Remove(propertyName);
		else
			_errors[propertyName] = validationResult.ToDictionary().Single().Value;
	}
}