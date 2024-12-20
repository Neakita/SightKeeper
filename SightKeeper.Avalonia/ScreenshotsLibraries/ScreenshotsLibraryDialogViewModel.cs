using System;
using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.ScreenshotsLibraries;
using SightKeeper.Avalonia.Dialogs;

namespace SightKeeper.Avalonia.ScreenshotsLibraries;

internal sealed partial class ScreenshotsLibraryDialogViewModel : DialogViewModel<bool>, ScreenshotsLibraryData, INotifyDataErrorInfo, IDisposable
{
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
	{
		add => _viewModelValidator.ErrorsChanged += value;
		remove => _viewModelValidator.ErrorsChanged -= value;
	}

	public override string Header { get; }

	public bool HasErrors => _viewModelValidator.HasErrors;

	public ScreenshotsLibraryDialogViewModel(string header, IValidator<ScreenshotsLibraryData> validator, string initialName = "", string initialDescription = "")
	{
		Header = header;
		_validator = validator;
		_name = initialName;
		_description = initialDescription;
		_viewModelValidator = new ViewModelValidator<ScreenshotsLibraryData>(validator, this, this);
		_viewModelValidator.ErrorsChanged += OnErrorsChanged;
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		return _viewModelValidator.GetErrors(propertyName);
	}

	public void Dispose()
	{
		_viewModelValidator.ErrorsChanged -= OnErrorsChanged;
		_viewModelValidator.Dispose();
	}

	protected override bool DefaultResult => false;

	private readonly IValidator<ScreenshotsLibraryData> _validator;
	private readonly ViewModelValidator<ScreenshotsLibraryData> _viewModelValidator;
	[ObservableProperty] private string _name;
	[ObservableProperty] private string _description;

	private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		ApplyCommand.NotifyCanExecuteChanged();
	}

	[RelayCommand(CanExecute = nameof(CanApply))]
	private void Apply()
	{
		Return(true);
	}

	private bool CanApply()
	{
		return _validator.Validate(this).IsValid;
	}
}